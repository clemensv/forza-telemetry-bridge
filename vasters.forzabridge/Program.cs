using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Azure;
using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.VisualBasic;
using Vasters.ForzaBridge.Producer.ForzaMotorsport;
using Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry;
using AvroEventFormatter = CloudNative.CloudEvents.Avro.AvroEventFormatter;

namespace Vasters.ForzaBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "ForzaBridge";
            app.Description = "Captures and forwards Forza Motorsports telemetry to Microsoft Azure Event Hubs or Microsoft Fabric Event Streams";
            app.HelpOption("-?|-h|--help");

            var ipAddressOption = app.Option<string>("-i|--ip <ipAddress>", "The IP address to bind the UDP listener to (defaults to 'any'; use 127.0.0.1 or this machine's IP address in Forza)", CommandOptionType.SingleValue);
            var portOption = app.Option<int>("-p|--port <port>", "The port to bind the UDP listener to (defaults to 5300)", CommandOptionType.SingleValue);
            var eventHubNamespaceOption = app.Option<string>("-n|--namespace <namespace>", "The Event Hub namespace (overrides -c)", CommandOptionType.SingleValue);
            var eventHubNameOption = app.Option<string>("-e|--eventhub <eventHubName>", "The Event Hub name (overrides -c)", CommandOptionType.SingleValue);
            var eventHubConnectionStringOption = app.Option<string?>("-c|--connection <connectionString>", "The Event Hub connection string", CommandOptionType.SingleValue);
            var eventEncodingOption = app.Option<EventDataEncoding>("-x|--dataEncoding <encoding>", "The data encoding to use (Json, AvroBinary, AvroJson, JsonGzip, AvroBinaryGzip, AvroJsonGzip; defaults to Json)", CommandOptionType.SingleValue);
            var cloudEventEncodingOption = app.Option<CloudEventEncoding>("-v|--cloudEventEncoding <cloudEventEncoding>", "The cloud event encoding to use (Binary, JsonStructured, AvroStructured; defaults to Binary)", CommandOptionType.SingleValue);
            var dataModeOption = app.Option<DataMode>("-d|--data <dataMode>", "The data mode to use (Sled, Dash, or Automatic; defaults to Automatic)", CommandOptionType.SingleValue);
            var dataRateOption = app.Option<int?>("-r|--rate <dataRate>", "The upstream data rate (in Hz, defaults to 1, max 60)", CommandOptionType.SingleValue);
            var tenantIdOption = app.Option<string>("-t|--tenant <tenantId>", "The tenant ID (defaults to 'default')", CommandOptionType.SingleValue);
            var sessionIdOption = app.Option<string>("-s|--session <sessionId>", "The session ID (defaults to date value 'YYMMDDHHMMSS')", CommandOptionType.SingleValue);
            var carIdOption = app.Option<string>("--car <carId>", "The car ID (defaults to CarOrdinal:CarClass:CarPerformanceIndex from telemetry, e.g. '45:5:500')", CommandOptionType.SingleValue);
            

            app.OnExecuteAsync(async (ct) =>
            {
                var ipAddress = (ipAddressOption.HasValue())?IPAddress.Parse(ipAddressOption.ParsedValue):IPAddress.Any;
                var port = (portOption.HasValue())?portOption.ParsedValue:5300;
                var dataMode = (dataModeOption.HasValue())?dataModeOption.ParsedValue:DataMode.Automatic;
                var eventHubNamespace = (eventHubNamespaceOption.HasValue()) ? eventHubNamespaceOption.ParsedValue : null;
                var eventHubName = (eventHubNameOption.HasValue())?eventHubNameOption.ParsedValue:null;
                var eventHubConnectionString = (eventHubConnectionStringOption.HasValue())?eventHubConnectionStringOption.ParsedValue:null;
                var eventEncoding = (eventEncodingOption.HasValue())?eventEncodingOption.ParsedValue:EventDataEncoding.Json;
                var cloudEventEncoding = (cloudEventEncodingOption.HasValue())?cloudEventEncodingOption.ParsedValue:CloudEventEncoding.Binary;
                var dataRate = dataRateOption.ParsedValue ?? 1;
                var tenantId = (tenantIdOption.HasValue()) ? tenantIdOption.ParsedValue : "default";
                var sessionId = (sessionIdOption.HasValue()) ? sessionIdOption.ParsedValue : DateTimeOffset.UtcNow.ToString("yyMMddHHmmss");
                var carId = (carIdOption.HasValue())?carIdOption.ParsedValue:null;

                string? eventHubPolicyName = null;
                string? eventHubPolicyKey = null;
                if ( eventHubConnectionString != null )
                {
                    var csProps = EventHubsConnectionStringProperties.Parse(eventHubConnectionString);
                    if ( csProps.FullyQualifiedNamespace != null && eventHubNamespace == null ) eventHubNamespace = csProps.Endpoint.Host;
                    if ( csProps.EventHubName != null && eventHubName == null ) eventHubName = csProps.EventHubName;
                    if ( csProps.SharedAccessKeyName != null ) eventHubPolicyName = csProps.SharedAccessKeyName;
                    if ( csProps.SharedAccessKey != null ) eventHubPolicyKey = csProps.SharedAccessKey;
                }

                if ( eventHubNamespace == null || eventHubName == null )
                {
                    Console.WriteLine("Please provide the Event Hub namespace. -c or -n/-e");
                    return 1;
                }

                if ( dataRate > 60 )
                {
                    Console.WriteLine("Data rate must be 60 Hz or less");
                    return 1;
                }

                var eventEncodingContentType = 
                    (eventEncoding == EventDataEncoding.Json) ? "application/json" :
                    (eventEncoding == EventDataEncoding.AvroBinary) ? "application/vnd.apache.avro+avro" :
                    (eventEncoding == EventDataEncoding.AvroJson) ? "application/vnd.apache.avro+json" :
                    (eventEncoding == EventDataEncoding.JsonGzip) ? "application/json+gzip" :
                    (eventEncoding == EventDataEncoding.AvroBinaryGzip) ? "application/vnd.apache.avro+avro+gzip":
                    (eventEncoding == EventDataEncoding.AvroJsonGzip) ? "application/vnd.apache.avro+json+gzip" :
                    throw new NotSupportedException($"Unsupported encoding {eventEncoding}");

                Console.WriteLine($"Starting ForzaBridge with data mode {dataMode} at {dataRate} Hz");
                Console.WriteLine($"Tenant: {tenantId}, Session: {sessionId}");
                
                var udpClient = new UdpClient();
                udpClient.Client.Bind(new IPEndPoint(ipAddress, port));

                Console.WriteLine($"Listening for Forza Motorsports telemetry on {ipAddress}:{port}");

                CloudEventFormatter? formatter = null;
                if ( cloudEventEncoding == CloudEventEncoding.JsonStructured )
                {
                    formatter = new JsonEventFormatter();
                }
                else if ( cloudEventEncoding == CloudEventEncoding.AvroStructured )
                {
                    formatter = new AvroEventFormatter();
                }

                EventHubProducerClient producerClient = (eventHubPolicyName != null && eventHubPolicyKey != null) ?
                    new EventHubProducerClient(eventHubNamespace, eventHubName, new AzureNamedKeyCredential(eventHubPolicyName, eventHubPolicyKey)) :
                    new EventHubProducerClient(eventHubNamespace, eventHubName, new DefaultAzureCredential());

                Console.WriteLine($"Sending telemetry to Event Hub {eventHubNamespace}/{eventHubName}, event encoding {eventEncodingContentType}");

                // This is the base offset for all timestamps produced based on the stopwatch timer. We'll get the time and zero out the msecs.
                // We don't care about being absolultely in sync with the wall clock, but we care about the relative time between events.
                DateTimeOffset startTime = DateTimeOffset.UtcNow;
                var startTimeEpoch = new DateTimeOffset(
                    startTime.Year, startTime.Month, startTime.Day, 
                    startTime.Hour, startTime.Minute, startTime.Second, 
                    0, startTime.Offset).ToUnixTimeMilliseconds();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var lastSend = stopwatch.ElapsedMilliseconds;

                // Initialize the lapId
                int lapId = 0;
                int priorLapId = 0;

                Dictionary<ChannelType, List<double>> channelData = InitializeChannelData();
                var sledDataSize = typeof(TelemetryDataSled).GetFields().Length * 4;
                while (true)
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    var receivedData = await udpClient.ReceiveAsync();
                    var effectiveDataMode = (dataMode == DataMode.Automatic) ? 
                        (receivedData.Buffer.Length <= sledDataSize ? DataMode.Sled : DataMode.Dash) : dataMode;
                    TelemetryDataSled telemetryData = (effectiveDataMode == DataMode.Dash) ?
                        ParseTelemetryData<TelemetryDataDash>(receivedData.Buffer, effectiveDataMode) :
                        ParseTelemetryData<TelemetryDataSled>(receivedData.Buffer, effectiveDataMode);

                    if (telemetryData.IsRaceOn == 0)
                    {
                        continue;
                    }
                    var timestamp = stopwatch.ElapsedMilliseconds;
                    // Normalize the timestamp to the start time
                    var normalizedTimestamp = startTimeEpoch + timestamp;

                    channelData[ChannelType.AccelerationX].Add(telemetryData.AccelerationX);
                    channelData[ChannelType.AccelerationY].Add(telemetryData.AccelerationY);
                    channelData[ChannelType.AccelerationZ].Add(telemetryData.AccelerationZ);
                    channelData[ChannelType.VelocityX].Add(telemetryData.VelocityX);
                    channelData[ChannelType.VelocityY].Add(telemetryData.VelocityY);
                    channelData[ChannelType.VelocityZ].Add(telemetryData.VelocityZ);
                    channelData[ChannelType.AngularVelocityX].Add(telemetryData.AngularVelocityX);
                    channelData[ChannelType.AngularVelocityY].Add(telemetryData.AngularVelocityY);
                    channelData[ChannelType.AngularVelocityZ].Add(telemetryData.AngularVelocityZ);
                    channelData[ChannelType.Yaw].Add(telemetryData.Yaw);
                    channelData[ChannelType.Pitch].Add(telemetryData.Pitch);
                    channelData[ChannelType.Roll].Add(telemetryData.Roll);
                    channelData[ChannelType.NormalizedSuspensionTravelFrontLeft].Add(telemetryData.NormalizedSuspensionTravelFrontLeft);
                    channelData[ChannelType.NormalizedSuspensionTravelFrontRight].Add(telemetryData.NormalizedSuspensionTravelFrontRight);
                    channelData[ChannelType.NormalizedSuspensionTravelRearLeft].Add(telemetryData.NormalizedSuspensionTravelRearLeft);
                    channelData[ChannelType.NormalizedSuspensionTravelRearRight].Add(telemetryData.NormalizedSuspensionTravelRearRight);
                    channelData[ChannelType.TireSlipRatioFrontLeft].Add(telemetryData.TireSlipRatioFrontLeft);
                    channelData[ChannelType.TireSlipRatioFrontRight].Add(telemetryData.TireSlipRatioFrontRight);
                    channelData[ChannelType.TireSlipRatioRearLeft].Add(telemetryData.TireSlipRatioRearLeft);
                    channelData[ChannelType.TireSlipRatioRearRight].Add(telemetryData.TireSlipRatioRearRight);
                    channelData[ChannelType.WheelRotationSpeedFrontLeft].Add(telemetryData.WheelRotationSpeedFrontLeft);
                    channelData[ChannelType.WheelRotationSpeedFrontRight].Add(telemetryData.WheelRotationSpeedFrontRight);
                    channelData[ChannelType.WheelRotationSpeedRearLeft].Add(telemetryData.WheelRotationSpeedRearLeft);
                    channelData[ChannelType.WheelRotationSpeedRearRight].Add(telemetryData.WheelRotationSpeedRearRight);
                    channelData[ChannelType.SurfaceRumbleFrontLeft].Add(telemetryData.SurfaceRumbleFrontLeft);
                    channelData[ChannelType.SurfaceRumbleFrontRight].Add(telemetryData.SurfaceRumbleFrontRight);
                    channelData[ChannelType.SurfaceRumbleRearLeft].Add(telemetryData.SurfaceRumbleRearLeft);
                    channelData[ChannelType.SurfaceRumbleRearRight].Add(telemetryData.SurfaceRumbleRearRight);
                    channelData[ChannelType.TireSlipAngleFrontLeft].Add(telemetryData.TireSlipAngleFrontLeft);
                    channelData[ChannelType.TireSlipAngleFrontRight].Add(telemetryData.TireSlipAngleFrontRight);
                    channelData[ChannelType.TireSlipAngleRearLeft].Add(telemetryData.TireSlipAngleRearLeft);
                    channelData[ChannelType.TireSlipAngleRearRight].Add(telemetryData.TireSlipAngleRearRight);
                    channelData[ChannelType.TireCombinedSlipFrontLeft].Add(telemetryData.TireCombinedSlipFrontLeft);
                    channelData[ChannelType.TireCombinedSlipFrontRight].Add(telemetryData.TireCombinedSlipFrontRight);
                    channelData[ChannelType.TireCombinedSlipRearLeft].Add(telemetryData.TireCombinedSlipRearLeft);
                    channelData[ChannelType.TireCombinedSlipRearRight].Add(telemetryData.TireCombinedSlipRearRight);
                    channelData[ChannelType.SuspensionTravelMetersFrontLeft].Add(telemetryData.SuspensionTravelMetersFrontLeft);
                    channelData[ChannelType.SuspensionTravelMetersFrontRight].Add(telemetryData.SuspensionTravelMetersFrontRight);
                    channelData[ChannelType.SuspensionTravelMetersRearLeft].Add(telemetryData.SuspensionTravelMetersRearLeft);
                    channelData[ChannelType.SuspensionTravelMetersRearRight].Add(telemetryData.SuspensionTravelMetersRearRight);
                    if (telemetryData is TelemetryDataDash)
                    {
                        var dash = (TelemetryDataDash)telemetryData;

                        channelData[ChannelType.PositionX].Add(dash.PositionX);
                        channelData[ChannelType.PositionY].Add(dash.PositionY);
                        channelData[ChannelType.PositionZ].Add(dash.PositionZ);
                        channelData[ChannelType.Speed].Add(dash.Speed);
                        channelData[ChannelType.Power].Add(dash.Power);
                        channelData[ChannelType.Torque].Add(dash.Torque);
                        channelData[ChannelType.TireTempFrontLeft].Add(dash.TireTempFrontLeft);
                        channelData[ChannelType.TireTempFrontRight].Add(dash.TireTempFrontRight);
                        channelData[ChannelType.TireTempRearLeft].Add(dash.TireTempRearLeft);
                        channelData[ChannelType.TireTempRearRight].Add(dash.TireTempRearRight);
                        channelData[ChannelType.Boost].Add(dash.Boost);
                        channelData[ChannelType.Fuel].Add(dash.Fuel);
                        channelData[ChannelType.DistanceTraveled].Add(dash.DistanceTraveled);
                        channelData[ChannelType.RacePosition].Add(dash.RacePosition);
                        channelData[ChannelType.Accel].Add(dash.Accel);
                        channelData[ChannelType.Brake].Add(dash.Brake);
                        channelData[ChannelType.Clutch].Add(dash.Clutch);
                        channelData[ChannelType.HandBrake].Add(dash.HandBrake);
                        channelData[ChannelType.Gear].Add(dash.Gear);
                        channelData[ChannelType.Steer].Add(dash.Steer);
                        channelData[ChannelType.NormalizedDrivingLine].Add(dash.NormalizedDrivingLine);
                        channelData[ChannelType.NormalizedAIBrakeDifference].Add(dash.NormalizedAIBrakeDifference);
                        channelData[ChannelType.TireWearFrontLeft].Add(dash.TireWearFrontLeft);
                        channelData[ChannelType.TireWearFrontRight].Add(dash.TireWearFrontRight);
                        channelData[ChannelType.TireWearRearLeft].Add(dash.TireWearRearLeft);
                        channelData[ChannelType.TireWearRearRight].Add(dash.TireWearRearRight);

                        lapId = dash.LapNumber;
                        if (priorLapId != lapId)
                        {
                            priorLapId = lapId;
                            Console.WriteLine($"lap: {lapId}");
                            var startTS = lastSend + startTimeEpoch;
                            var endTS = timestamp + startTimeEpoch;
                            var effectiveLapId = lapId.ToString();
                            var effectiveCarId = (carId != null) ? carId : $"{telemetryData.CarOrdinal}:{telemetryData.CarClass}:{telemetryData.CarPerformanceIndex}";
                            _ = Task.Run(() => SendLapSignal(producerClient, startTS, endTS, tenantId, effectiveCarId, sessionId, effectiveLapId, eventEncodingContentType, formatter));
                        }
                    }

                    if (timestamp - lastSend >= (1000 / dataRate)-1)
                    {
                        var capturedChannelData = channelData;
                        channelData = InitializeChannelData();
                        Console.WriteLine($"lap: {lapId}, msec: {timestamp - lastSend}, recs: {capturedChannelData[ChannelType.AccelerationX].Count}");
                        var startTS = lastSend + startTimeEpoch;
                        var endTS = timestamp + startTimeEpoch;
                        var effectiveLapId = lapId.ToString();
                        var effectiveCarId = (carId != null) ? carId : $"{telemetryData.CarOrdinal}:{telemetryData.CarClass}:{telemetryData.CarPerformanceIndex}";
                        lastSend = timestamp;
                        _ = Task.Run(() => SendChannelData(producerClient, capturedChannelData, startTS, endTS, tenantId, effectiveCarId, sessionId, effectiveLapId, eventEncodingContentType, formatter));
                    }
                }
            });
            app.Execute(args);
        }

        public enum EventDataEncoding
        {
            Json,
            AvroBinary,
            AvroJson,
            JsonGzip,
            AvroBinaryGzip,
            AvroJsonGzip
        }

        public enum CloudEventEncoding
        {
            Binary,
            JsonStructured,
            AvroStructured
        }

        private static async Task SendLapSignal(EventHubProducerClient producerClient, long lastSend, long timestamp, 
                                                string tenantId, string carId, string sessionId, 
                                                string lapId, string contentType, CloudEventFormatter? formatter)
        {
            var lapSignalEvent = TelemetryEventFactory.CreateLapSignalEvent(new LapSignal
            {
                CarId = carId,
                SessionId = sessionId,
                LapId = lapId
            }, tenantId, carId, sessionId, contentType, formatter);
            await producerClient.SendAsync(new[] { lapSignalEvent });
            Console.WriteLine($"Sent lap signal event for car {carId}, lap {lapId}");
        }

        private static async Task SendChannelData(EventHubProducerClient producerClient, Dictionary<ChannelType, List<double>> capturedChannelData, 
                                                  long startTS, long endTS, string tenantId, string carId, string sessionId,
                                                  string lapId, string contentType, CloudEventFormatter? formatter)
        {
            int totalEventCount = 0;
            var batch = await producerClient.CreateBatchAsync();
            foreach (var channelData in capturedChannelData)
            {
                if (channelData.Value.Count == 0)
                {
                    continue;
                }
                var channelEvent = TelemetryEventFactory.CreateChannelEvent(new Channel
                {
                    ChannelId = channelData.Key,
                    CarId = carId,
                    SessionId = sessionId,
                    LapId = lapId,
                    SampleCount = channelData.Value.Count,
                    Frequency = (int)(channelData.Value.Count / ((endTS - startTS)/1000.0)),
                    Timespan = new BatchTimespan
                    {
                        StartTS = startTS,
                        EndTS = endTS
                    },
                    Data = channelData.Value
                }, tenantId, carId, channelData.Key.ToString(), contentType, formatter);
                if ( !batch.TryAdd(channelEvent) )
                {
                    await producerClient.SendAsync(batch);
                    totalEventCount += batch.Count;
                    batch = await producerClient.CreateBatchAsync();
                    batch.TryAdd(channelEvent);
                }
            }
            if (batch.Count > 0)
            {
                await producerClient.SendAsync(batch);
                totalEventCount += batch.Count;
            }
            Console.WriteLine($"Sent {totalEventCount} channel events for car {carId}");
        }

        private static Dictionary<ChannelType, List<double>> InitializeChannelData()
        {
            var channelData = new Dictionary<ChannelType, List<double>>();
            channelData.Add(ChannelType.AccelerationX, new List<double>());
            channelData.Add(ChannelType.AccelerationY, new List<double>());
            channelData.Add(ChannelType.AccelerationZ, new List<double>());
            channelData.Add(ChannelType.VelocityX, new List<double>());
            channelData.Add(ChannelType.VelocityY, new List<double>());
            channelData.Add(ChannelType.VelocityZ, new List<double>());
            channelData.Add(ChannelType.AngularVelocityX, new List<double>());
            channelData.Add(ChannelType.AngularVelocityY, new List<double>());
            channelData.Add(ChannelType.AngularVelocityZ, new List<double>());
            channelData.Add(ChannelType.Yaw, new List<double>());
            channelData.Add(ChannelType.Pitch, new List<double>());
            channelData.Add(ChannelType.Roll, new List<double>());
            channelData.Add(ChannelType.NormalizedSuspensionTravelFrontLeft, new List<double>());
            channelData.Add(ChannelType.NormalizedSuspensionTravelFrontRight, new List<double>());
            channelData.Add(ChannelType.NormalizedSuspensionTravelRearLeft, new List<double>());
            channelData.Add(ChannelType.NormalizedSuspensionTravelRearRight, new List<double>());
            channelData.Add(ChannelType.TireSlipRatioFrontLeft, new List<double>());
            channelData.Add(ChannelType.TireSlipRatioFrontRight, new List<double>());
            channelData.Add(ChannelType.TireSlipRatioRearLeft, new List<double>());
            channelData.Add(ChannelType.TireSlipRatioRearRight, new List<double>());
            channelData.Add(ChannelType.WheelRotationSpeedFrontLeft, new List<double>());
            channelData.Add(ChannelType.WheelRotationSpeedFrontRight, new List<double>());
            channelData.Add(ChannelType.WheelRotationSpeedRearLeft, new List<double>());
            channelData.Add(ChannelType.WheelRotationSpeedRearRight, new List<double>());
            channelData.Add(ChannelType.SurfaceRumbleFrontLeft, new List<double>());
            channelData.Add(ChannelType.SurfaceRumbleFrontRight, new List<double>());
            channelData.Add(ChannelType.SurfaceRumbleRearLeft, new List<double>());
            channelData.Add(ChannelType.SurfaceRumbleRearRight, new List<double>());
            channelData.Add(ChannelType.TireSlipAngleFrontLeft, new List<double>());
            channelData.Add(ChannelType.TireSlipAngleFrontRight, new List<double>());
            channelData.Add(ChannelType.TireSlipAngleRearLeft, new List<double>());
            channelData.Add(ChannelType.TireSlipAngleRearRight, new List<double>());
            channelData.Add(ChannelType.TireCombinedSlipFrontLeft, new List<double>());
            channelData.Add(ChannelType.TireCombinedSlipFrontRight, new List<double>());
            channelData.Add(ChannelType.TireCombinedSlipRearLeft, new List<double>());
            channelData.Add(ChannelType.TireCombinedSlipRearRight, new List<double>());
            channelData.Add(ChannelType.SuspensionTravelMetersFrontLeft, new List<double>());
            channelData.Add(ChannelType.SuspensionTravelMetersFrontRight, new List<double>());
            channelData.Add(ChannelType.SuspensionTravelMetersRearLeft, new List<double>());
            channelData.Add(ChannelType.SuspensionTravelMetersRearRight, new List<double>());
            channelData.Add(ChannelType.PositionX, new List<double>());
            channelData.Add(ChannelType.PositionY, new List<double>());
            channelData.Add(ChannelType.PositionZ, new List<double>());
            channelData.Add(ChannelType.Speed, new List<double>());
            channelData.Add(ChannelType.Power, new List<double>());
            channelData.Add(ChannelType.Torque, new List<double>());
            channelData.Add(ChannelType.TireTempFrontLeft, new List<double>());
            channelData.Add(ChannelType.TireTempFrontRight, new List<double>());
            channelData.Add(ChannelType.TireTempRearLeft, new List<double>());
            channelData.Add(ChannelType.TireTempRearRight, new List<double>());
            channelData.Add(ChannelType.Boost, new List<double>());
            channelData.Add(ChannelType.Fuel, new List<double>());
            channelData.Add(ChannelType.DistanceTraveled, new List<double>());
            channelData.Add(ChannelType.RacePosition, new List<double>());
            channelData.Add(ChannelType.Accel, new List<double>());
            channelData.Add(ChannelType.Brake, new List<double>());
            channelData.Add(ChannelType.Clutch, new List<double>());
            channelData.Add(ChannelType.HandBrake, new List<double>());
            channelData.Add(ChannelType.Gear, new List<double>());
            channelData.Add(ChannelType.Steer, new List<double>());
            channelData.Add(ChannelType.NormalizedDrivingLine, new List<double>());
            channelData.Add(ChannelType.NormalizedAIBrakeDifference, new List<double>());
            channelData.Add(ChannelType.TireWearFrontLeft, new List<double>());
            channelData.Add(ChannelType.TireWearFrontRight, new List<double>());
            channelData.Add(ChannelType.TireWearRearLeft, new List<double>());
            channelData.Add(ChannelType.TireWearRearRight, new List<double>());

            return channelData;
        }

        static T ParseTelemetryData<T>(byte[] data, DataMode dataMode) where T : TelemetryDataSled
        {
            // Implement the logic to parse the telemetry data from the byte array
            // and return the parsed telemetry data object. Base this on the Forza Motorsports docs:
            // https://support.forzamotorsport.net/hc/en-us/articles/21742934024211-Forza-Motorsport-Data-Out-Documentation

            TelemetryDataSled telemetryData = (dataMode == DataMode.Sled) ? new TelemetryDataSled() : new TelemetryDataDash();
            // decode the data
            var stream = new System.IO.MemoryStream(data);
            var reader = new System.IO.BinaryReader(stream);
            telemetryData.IsRaceOn = reader.ReadInt32();
            telemetryData.TimestampMS = reader.ReadUInt32();
            telemetryData.EngineMaxRpm = reader.ReadSingle();
            telemetryData.EngineIdleRpm = reader.ReadSingle();
            telemetryData.CurrentEngineRpm = reader.ReadSingle();
            telemetryData.AccelerationX = reader.ReadSingle();
            telemetryData.AccelerationY = reader.ReadSingle();
            telemetryData.AccelerationZ = reader.ReadSingle();
            telemetryData.VelocityX = reader.ReadSingle();
            telemetryData.VelocityY = reader.ReadSingle();
            telemetryData.VelocityZ = reader.ReadSingle();
            telemetryData.AngularVelocityX = reader.ReadSingle();
            telemetryData.AngularVelocityY = reader.ReadSingle();
            telemetryData.AngularVelocityZ = reader.ReadSingle();
            telemetryData.Yaw = reader.ReadSingle();
            telemetryData.Pitch = reader.ReadSingle();
            telemetryData.Roll = reader.ReadSingle();
            telemetryData.NormalizedSuspensionTravelFrontLeft = reader.ReadSingle();
            telemetryData.NormalizedSuspensionTravelFrontRight = reader.ReadSingle();
            telemetryData.NormalizedSuspensionTravelRearLeft = reader.ReadSingle();
            telemetryData.NormalizedSuspensionTravelRearRight = reader.ReadSingle();
            telemetryData.TireSlipRatioFrontLeft = reader.ReadSingle();
            telemetryData.TireSlipRatioFrontRight = reader.ReadSingle();
            telemetryData.TireSlipRatioRearLeft = reader.ReadSingle();
            telemetryData.TireSlipRatioRearRight = reader.ReadSingle();
            telemetryData.WheelRotationSpeedFrontLeft = reader.ReadSingle();
            telemetryData.WheelRotationSpeedFrontRight = reader.ReadSingle();
            telemetryData.WheelRotationSpeedRearLeft = reader.ReadSingle();
            telemetryData.WheelRotationSpeedRearRight = reader.ReadSingle();
            telemetryData.WheelOnRumbleStripFrontLeft = reader.ReadInt32();
            telemetryData.WheelOnRumbleStripFrontRight = reader.ReadInt32();
            telemetryData.WheelOnRumbleStripRearLeft = reader.ReadInt32();
            telemetryData.WheelOnRumbleStripRearRight = reader.ReadInt32();
            telemetryData.WheelInPuddleDepthFrontLeft = reader.ReadSingle();
            telemetryData.WheelInPuddleDepthFrontRight = reader.ReadSingle();
            telemetryData.WheelInPuddleDepthRearLeft = reader.ReadSingle();
            telemetryData.WheelInPuddleDepthRearRight = reader.ReadSingle();
            telemetryData.SurfaceRumbleFrontLeft = reader.ReadSingle();
            telemetryData.SurfaceRumbleFrontRight = reader.ReadSingle();
            telemetryData.SurfaceRumbleRearLeft = reader.ReadSingle();
            telemetryData.SurfaceRumbleRearRight = reader.ReadSingle();
            telemetryData.TireSlipAngleFrontLeft = reader.ReadSingle();
            telemetryData.TireSlipAngleFrontRight = reader.ReadSingle();
            telemetryData.TireSlipAngleRearLeft = reader.ReadSingle();
            telemetryData.TireSlipAngleRearRight = reader.ReadSingle();
            telemetryData.TireCombinedSlipFrontLeft = reader.ReadSingle();
            telemetryData.TireCombinedSlipFrontRight = reader.ReadSingle();
            telemetryData.TireCombinedSlipRearLeft = reader.ReadSingle();
            telemetryData.TireCombinedSlipRearRight = reader.ReadSingle();
            telemetryData.SuspensionTravelMetersFrontLeft = reader.ReadSingle();
            telemetryData.SuspensionTravelMetersFrontRight = reader.ReadSingle();
            telemetryData.SuspensionTravelMetersRearLeft = reader.ReadSingle();
            telemetryData.SuspensionTravelMetersRearRight = reader.ReadSingle();
            telemetryData.CarOrdinal = reader.ReadInt32();
            telemetryData.CarClass = reader.ReadInt32();
            telemetryData.CarPerformanceIndex = reader.ReadInt32();
            telemetryData.DrivetrainType = reader.ReadInt32();
            telemetryData.NumCylinders = reader.ReadInt32();
            if (telemetryData is TelemetryDataDash)
            {
                TelemetryDataDash dash = (TelemetryDataDash)telemetryData;
                dash.PositionX = reader.ReadSingle();
                dash.PositionY = reader.ReadSingle();
                dash.PositionZ = reader.ReadSingle();
                dash.Speed = reader.ReadSingle();
                dash.Power = reader.ReadSingle();
                dash.Torque = reader.ReadSingle();
                dash.TireTempFrontLeft = reader.ReadSingle();
                dash.TireTempFrontRight = reader.ReadSingle();
                dash.TireTempRearLeft = reader.ReadSingle();
                dash.TireTempRearRight = reader.ReadSingle();
                dash.Boost = reader.ReadSingle();
                dash.Fuel = reader.ReadSingle();
                dash.DistanceTraveled = reader.ReadSingle();
                dash.BestLap = reader.ReadSingle();
                dash.LastLap = reader.ReadSingle();
                dash.CurrentLap = reader.ReadSingle();
                dash.CurrentRaceTime = reader.ReadSingle();
                dash.LapNumber = reader.ReadUInt16();
                dash.RacePosition = reader.ReadByte();
                dash.Accel = reader.ReadByte();
                dash.Brake = reader.ReadByte();
                dash.Clutch = reader.ReadByte();
                dash.HandBrake = reader.ReadByte();
                dash.Gear = reader.ReadByte();
                dash.Steer = reader.ReadSByte();
                dash.NormalizedDrivingLine = reader.ReadSByte();
                dash.NormalizedAIBrakeDifference = reader.ReadSByte();
                dash.TireWearFrontLeft = reader.ReadSingle();
                dash.TireWearFrontRight = reader.ReadSingle();
                dash.TireWearRearLeft = reader.ReadSingle();
                dash.TireWearRearRight = reader.ReadSingle();
                dash.TrackOrdinal = reader.ReadInt32();
            }
            return (T)telemetryData;
        }
    }

    public enum DataMode
    {
        Sled,
        Dash,
        Automatic
    }


    public class TelemetryDataSled
    {
        public int IsRaceOn; // S32
        public uint TimestampMS; // U32
        public float EngineMaxRpm; // F32
        public float EngineIdleRpm; // F32
        public float CurrentEngineRpm; // F32
        public float AccelerationX; // F32
        public float AccelerationY; // F32
        public float AccelerationZ; // F32
        public float VelocityX; // F32
        public float VelocityY; // F32
        public float VelocityZ; // F32
        public float AngularVelocityX; // F32
        public float AngularVelocityY; // F32
        public float AngularVelocityZ; // F32
        public float Yaw; // F32
        public float Pitch; // F32
        public float Roll; // F32
        public float NormalizedSuspensionTravelFrontLeft; // F32
        public float NormalizedSuspensionTravelFrontRight; // F32
        public float NormalizedSuspensionTravelRearLeft; // F32
        public float NormalizedSuspensionTravelRearRight; // F32
        public float TireSlipRatioFrontLeft; // F32
        public float TireSlipRatioFrontRight; // F32
        public float TireSlipRatioRearLeft; // F32
        public float TireSlipRatioRearRight; // F32
        public float WheelRotationSpeedFrontLeft; // F32
        public float WheelRotationSpeedFrontRight; // F32
        public float WheelRotationSpeedRearLeft; // F32
        public float WheelRotationSpeedRearRight; // F32
        public int WheelOnRumbleStripFrontLeft; // S32
        public int WheelOnRumbleStripFrontRight; // S32
        public int WheelOnRumbleStripRearLeft; // S32
        public int WheelOnRumbleStripRearRight; // S32
        public float WheelInPuddleDepthFrontLeft; // F32
        public float WheelInPuddleDepthFrontRight; // F32
        public float WheelInPuddleDepthRearLeft; // F32
        public float WheelInPuddleDepthRearRight; // F32
        public float SurfaceRumbleFrontLeft; // F32
        public float SurfaceRumbleFrontRight; // F32
        public float SurfaceRumbleRearLeft; // F32
        public float SurfaceRumbleRearRight; // F32
        public float TireSlipAngleFrontLeft; // F32
        public float TireSlipAngleFrontRight; // F32
        public float TireSlipAngleRearLeft; // F32
        public float TireSlipAngleRearRight; // F32
        public float TireCombinedSlipFrontLeft; // F32
        public float TireCombinedSlipFrontRight; // F32
        public float TireCombinedSlipRearLeft; // F32
        public float TireCombinedSlipRearRight; // F32
        public float SuspensionTravelMetersFrontLeft; // F32
        public float SuspensionTravelMetersFrontRight; // F32
        public float SuspensionTravelMetersRearLeft; // F32
        public float SuspensionTravelMetersRearRight; // F32
        public int CarOrdinal; // S32
        public int CarClass; // S32
        public int CarPerformanceIndex; // S32
        public int DrivetrainType; // S32
        public int NumCylinders; // S32
    }

    public class TelemetryDataDash : TelemetryDataSled
    {
        public float PositionX; // F32
        public float PositionY; // F32
        public float PositionZ; // F32
        public float Speed; // F32
        public float Power; // F32
        public float Torque; // F32
        public float TireTempFrontLeft; // F32
        public float TireTempFrontRight; // F32
        public float TireTempRearLeft; // F32
        public float TireTempRearRight; // F32
        public float Boost; // F32
        public float Fuel; // F32
        public float DistanceTraveled; // F32
        public float BestLap; // F32
        public float LastLap; // F32
        public float CurrentLap; // F32
        public float CurrentRaceTime; // F32
        public ushort LapNumber; // U16
        public byte RacePosition; // U8
        public byte Accel; // U8
        public byte Brake; // U8
        public byte Clutch; // U8
        public byte HandBrake; // U8
        public byte Gear; // U8
        public sbyte Steer; // S8
        public sbyte NormalizedDrivingLine; // S8
        public sbyte NormalizedAIBrakeDifference; // S8
        public float TireWearFrontLeft; // F32
        public float TireWearFrontRight; // F32
        public float TireWearRearLeft; // F32
        public float TireWearRearRight; // F32
        public int TrackOrdinal; // S32
    }
}
