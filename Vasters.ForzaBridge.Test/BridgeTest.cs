using Azure.Core.Diagnostics;
using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using Testcontainers.Azurite;
using Xunit.Abstractions;

namespace Vasters.ForzaBridge.Tests
{
    public class ForzaBridgeFixture : IAsyncLifetime
    {
        public AzuriteContainer? AzuriteContainer { get; private set; }
        public IContainer? EmulatorContainer { get; private set; }
        public INetwork? Network { get; private set; }
        public string? EventHubConnectionString { get; private set; }
        public string? BlobStorageConnectionString { get; private set; }
        public BlobContainerClient? BlobClient { get; private set; }
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;
        private string? emulatorConfigFilePath;
        private const string emulatorConfig = @"{
                      ""UserConfig"": {
                        ""NamespaceConfig"": [
                          {
                            ""Type"": ""EventHub"",
                            ""Name"": ""emulatorNs1"",
                            ""Entities"": [
                              {
                                ""Name"": ""eh1"",
                                ""PartitionCount"": ""2"",
                                ""ConsumerGroups"": [
                                  {
                                    ""Name"": ""cg1""
                                  }
                                ]
                              }
                            ]
                          }
                        ], 
                        ""LoggingConfig"": {
                          ""Type"": ""File""
                        }
                      }
                    }";

        public ForzaBridgeFixture()
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug().AddConsole();
            });
            _logger = _loggerFactory.CreateLogger<ForzaBridgeFixture>();
        }

        public ILogger Logger => _logger;

        public async Task InitializeAsync()
        {
            try
            {
                Network = new NetworkBuilder()
                    .WithName(Guid.NewGuid().ToString("D"))
                    .Build();
                emulatorConfigFilePath = Path.GetTempFileName();
                File.WriteAllText(emulatorConfigFilePath, emulatorConfig);
                AzuriteContainer = new AzuriteBuilder()
                    .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
                    .WithCommand("--skipApiVersionCheck")
                    .WithCommand("--loose")
                    .WithNetwork(Network)
                    .WithNetworkAliases("azurite")
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(".*Azurite Blob service is successfully listening"))
                    .Build();

                EmulatorContainer = new ContainerBuilder()
                        .WithImage("mcr.microsoft.com/azure-messaging/eventhubs-emulator:latest")
                        .WithBindMount(emulatorConfigFilePath, "/Eventhubs_Emulator/ConfigFiles/Config.json")
                        .WithPortBinding(5672, false)
                        .WithNetwork(Network)
                        .WithNetworkAliases("eventhubs-emulator")
                        .WithEnvironment("BLOB_SERVER", "azurite")
                        .WithEnvironment("METADATA_SERVER", "azurite")
                        .WithEnvironment("ACCEPT_EULA", "Y")
                        .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(".*Emulator is launching with config"))
                        .Build();

                await Network.CreateAsync();
                await AzuriteContainer.StartAsync();
                await EmulatorContainer.StartAsync();
                EventHubConnectionString = $"Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;EntityPath=eh1;";
                BlobStorageConnectionString = AzuriteContainer.GetConnectionString();
                BlobClient = new BlobContainerClient(BlobStorageConnectionString, "eventhub-checkpoints-123");
                if (!await BlobClient.ExistsAsync())
                {
                    await BlobClient.CreateAsync();
                    if (!await BlobClient.ExistsAsync())
                    {
                        throw new Exception("Blob container not created");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during InitializeAsync");
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            try
            {
                if (EmulatorContainer != null)
                {
                    await EmulatorContainer.StopAsync();
                }
                if (AzuriteContainer != null)
                {
                    await AzuriteContainer.StopAsync();
                }
                if (Network != null)
                {
                    await Network.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during DisposeAsync");
            }
            finally
            {
                if (emulatorConfigFilePath != null && File.Exists(emulatorConfigFilePath))
                {
                    File.Delete(emulatorConfigFilePath);
                }
            }
        }
    }

    public class ForzaBridgeTests : IClassFixture<ForzaBridgeFixture>
    {
        private readonly ForzaBridgeFixture _fixture;
        private ITestOutputHelper _output;


        public ForzaBridgeTests(ForzaBridgeFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }

        [Fact]
        public async Task TestTelemetryProcessing()
        {
            AzureEventSourceListener.CreateConsoleLogger();

            // Arrange
            var telemetryData = new TelemetryDataDash
            {
                IsRaceOn = 1,
                TimestampMS = 123456,
                EngineMaxRpm = 9000.0f,
                EngineIdleRpm = 1000.0f,
                CurrentEngineRpm = 7000.0f,
                AccelerationX = 0.1f,
                AccelerationY = 0.2f,
                AccelerationZ = 0.3f,
                VelocityX = 1.1f,
                VelocityY = 1.2f,
                VelocityZ = 1.3f,
                AngularVelocityX = 2.1f,
                AngularVelocityY = 2.2f,
                AngularVelocityZ = 2.3f,
                Yaw = 0.5f,
                Pitch = 0.6f,
                Roll = 0.7f,
                NormalizedSuspensionTravelFrontLeft = 0.8f,
                NormalizedSuspensionTravelFrontRight = 0.9f,
                NormalizedSuspensionTravelRearLeft = 1.0f,
                NormalizedSuspensionTravelRearRight = 1.1f,
                TireSlipRatioFrontLeft = 1.2f,
                TireSlipRatioFrontRight = 1.3f,
                TireSlipRatioRearLeft = 1.4f,
                TireSlipRatioRearRight = 1.5f,
                WheelRotationSpeedFrontLeft = 2.1f,
                WheelRotationSpeedFrontRight = 2.2f,
                WheelRotationSpeedRearLeft = 2.3f,
                WheelRotationSpeedRearRight = 2.4f,
                WheelOnRumbleStripFrontLeft = 1,
                WheelOnRumbleStripFrontRight = 0,
                WheelOnRumbleStripRearLeft = 0,
                WheelOnRumbleStripRearRight = 1,
                WheelInPuddleDepthFrontLeft = 0.05f,
                WheelInPuddleDepthFrontRight = 0.06f,
                WheelInPuddleDepthRearLeft = 0.07f,
                WheelInPuddleDepthRearRight = 0.08f,
                SurfaceRumbleFrontLeft = 3.1f,
                SurfaceRumbleFrontRight = 3.2f,
                SurfaceRumbleRearLeft = 3.3f,
                SurfaceRumbleRearRight = 3.4f,
                TireSlipAngleFrontLeft = 4.1f,
                TireSlipAngleFrontRight = 4.2f,
                TireSlipAngleRearLeft = 4.3f,
                TireSlipAngleRearRight = 4.4f,
                TireCombinedSlipFrontLeft = 5.1f,
                TireCombinedSlipFrontRight = 5.2f,
                TireCombinedSlipRearLeft = 5.3f,
                TireCombinedSlipRearRight = 5.4f,
                SuspensionTravelMetersFrontLeft = 6.1f,
                SuspensionTravelMetersFrontRight = 6.2f,
                SuspensionTravelMetersRearLeft = 6.3f,
                SuspensionTravelMetersRearRight = 6.4f,
                PositionX = 10.0f,
                PositionY = 11.0f,
                PositionZ = 12.0f,
                Speed = 100.0f,
                Power = 200.0f,
                Torque = 300.0f,
                TireTempFrontLeft = 50.0f,
                TireTempFrontRight = 51.0f,
                TireTempRearLeft = 52.0f,
                TireTempRearRight = 53.0f,
                Boost = 1.0f,
                Fuel = 20.0f,
                DistanceTraveled = 1234.0f,
                BestLap = 120.5f,
                LastLap = 121.0f,
                CurrentLap = 122.5f,
                CurrentRaceTime = 600.0f,
                LapNumber = 5,
                RacePosition = 1,
                Accel = 100,
                Brake = 100,
                Clutch = 100,
                HandBrake = 100,
                Gear = 3,
                Steer = 10,
                NormalizedDrivingLine = 0,
                NormalizedAIBrakeDifference = 0,
                TireWearFrontLeft = 0.1f,
                TireWearFrontRight = 0.2f,
                TireWearRearLeft = 0.3f,
                TireWearRearRight = 0.4f,
                TrackOrdinal = 1,
                CarOrdinal = 1,
                CarClass = 2,
                CarPerformanceIndex = 700,
                DrivetrainType = 1,
                NumCylinders = 8
            };

           var completionSource = new TaskCompletionSource<bool>();
            var processedEvents = new List<EventData>();
            // Assert
            var eventProcessor = new EventProcessorClient(
                new BlobContainerClient(_fixture.BlobStorageConnectionString, "eventhub-checkpoints-123"),
                "$Default",
                _fixture.EventHubConnectionString,
                "eh1");
            try
            {
                eventProcessor.ProcessEventAsync += (processEventArgs) =>
                {
                    if (processEventArgs.HasEvent)
                    {
                        processedEvents.Add(processEventArgs.Data);
                    }
                    completionSource.SetResult(true);
                    return Task.CompletedTask;

                };
                eventProcessor.ProcessErrorAsync += (errorEventArgs) =>
                {
                    _fixture.Logger.LogError($"Error on partition {errorEventArgs.PartitionId}: {errorEventArgs.Exception.Message}");
                    completionSource.SetException(errorEventArgs.Exception);
                    return Task.CompletedTask;
                };
                await eventProcessor.StartProcessingAsync();
                _fixture.Logger.LogInformation("EventProcessor started");

                string[] args = new[]
                {
                    "-i", "127.0.0.1",
                    "-p", "5300",
                    "-c", _fixture.EventHubConnectionString!,
                    "-e", "eh1",
                    "-x", "Json",
                    "-v", "Binary",
                    "-d", "Dash",
                    "-r", "60",
                    "-t", "default",
                    "-s", DateTimeOffset.UtcNow.ToString("yyMMddHHmmss"),
                    "--car", "45:5:500"
                };
                TaskCompletionSource<bool> appRunning = new TaskCompletionSource<bool>();
                var mainApp = new Thread(() =>
                {
                    try
                    {
                        if (Bridge.Main(args) != 0)
                        {
                            appRunning.SetException(new Exception("Main returned non-zero"));
                        }
                        else
                        {
                            appRunning.SetResult(true);
                        }
                    }
                    catch (ThreadInterruptedException)
                    {
                        appRunning.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        appRunning.SetException(ex);
                    }
                });
                mainApp.Start();
                try
                {
                    // startup
                    await Task.Delay(1000);

                    var udpClient = new UdpClient();

                    // Act
                    await SimulateTelemetryData(udpClient, telemetryData);
                    // wait out batch threshold
                    await Task.Delay(1000);
                    // Act
                    await SimulateTelemetryData(udpClient, telemetryData);
#pragma warning disable xUnit1031 // Do not use blocking task operations in test method
                    Assert.True(Task.WaitAll([completionSource.Task], 5000));
#pragma warning restore xUnit1031 // Do not use blocking task operations in test method

                    // Validate the events
                    Assert.NotEmpty(processedEvents);
                    foreach (var ev in processedEvents)
                    {
                        if (ev.Properties.TryGetValue("cloudEvents_type", out var ce_type))
                        {
                            Assert.True(ce_type.Equals("ForzaMotorsport.Telemetry.Channel") || ce_type.Equals("ForzaMotorsport.Telemetry.LapSignal"));
                        }
                    }
                }
                finally
                {
                    mainApp.Interrupt();
                    await appRunning.Task;
                }
            }
            finally
            {
                await eventProcessor.StopProcessingAsync();
            }
        }

        private async Task SimulateTelemetryData(UdpClient udpClient, TelemetryDataDash telemetryData)
        {
            var endpoint = new IPEndPoint(IPAddress.Loopback, 5300);
            for (var i = 0; i < 1000; i++)
            {
                var data = SerializeTelemetryData(telemetryData);
                await udpClient.SendAsync(data, data.Length, endpoint);
            }
        }

        private byte[] SerializeTelemetryData(TelemetryDataDash telemetryData)
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            writer.Write(telemetryData.IsRaceOn);
            writer.Write(telemetryData.TimestampMS);
            writer.Write(telemetryData.EngineMaxRpm);
            writer.Write(telemetryData.EngineIdleRpm);
            writer.Write(telemetryData.CurrentEngineRpm);
            writer.Write(telemetryData.AccelerationX);
            writer.Write(telemetryData.AccelerationY);
            writer.Write(telemetryData.AccelerationZ);
            writer.Write(telemetryData.VelocityX);
            writer.Write(telemetryData.VelocityY);
            writer.Write(telemetryData.VelocityZ);
            writer.Write(telemetryData.AngularVelocityX);
            writer.Write(telemetryData.AngularVelocityY);
            writer.Write(telemetryData.AngularVelocityZ);
            writer.Write(telemetryData.Yaw);
            writer.Write(telemetryData.Pitch);
            writer.Write(telemetryData.Roll);
            writer.Write(telemetryData.NormalizedSuspensionTravelFrontLeft);
            writer.Write(telemetryData.NormalizedSuspensionTravelFrontRight);
            writer.Write(telemetryData.NormalizedSuspensionTravelRearLeft);
            writer.Write(telemetryData.NormalizedSuspensionTravelRearRight);
            writer.Write(telemetryData.TireSlipRatioFrontLeft);
            writer.Write(telemetryData.TireSlipRatioFrontRight);
            writer.Write(telemetryData.TireSlipRatioRearLeft);
            writer.Write(telemetryData.TireSlipRatioRearRight);
            writer.Write(telemetryData.WheelRotationSpeedFrontLeft);
            writer.Write(telemetryData.WheelRotationSpeedFrontRight);
            writer.Write(telemetryData.WheelRotationSpeedRearLeft);
            writer.Write(telemetryData.WheelRotationSpeedRearRight);
            writer.Write(telemetryData.WheelOnRumbleStripFrontLeft);
            writer.Write(telemetryData.WheelOnRumbleStripFrontRight);
            writer.Write(telemetryData.WheelOnRumbleStripRearLeft);
            writer.Write(telemetryData.WheelOnRumbleStripRearRight);
            writer.Write(telemetryData.WheelInPuddleDepthFrontLeft);
            writer.Write(telemetryData.WheelInPuddleDepthFrontRight);
            writer.Write(telemetryData.WheelInPuddleDepthRearLeft);
            writer.Write(telemetryData.WheelInPuddleDepthRearRight);
            writer.Write(telemetryData.SurfaceRumbleFrontLeft);
            writer.Write(telemetryData.SurfaceRumbleFrontRight);
            writer.Write(telemetryData.SurfaceRumbleRearLeft);
            writer.Write(telemetryData.SurfaceRumbleRearRight);
            writer.Write(telemetryData.TireSlipAngleFrontLeft);
            writer.Write(telemetryData.TireSlipAngleFrontRight);
            writer.Write(telemetryData.TireSlipAngleRearLeft);
            writer.Write(telemetryData.TireSlipAngleRearRight);
            writer.Write(telemetryData.TireCombinedSlipFrontLeft);
            writer.Write(telemetryData.TireCombinedSlipFrontRight);
            writer.Write(telemetryData.TireCombinedSlipRearLeft);
            writer.Write(telemetryData.TireCombinedSlipRearRight);
            writer.Write(telemetryData.SuspensionTravelMetersFrontLeft);
            writer.Write(telemetryData.SuspensionTravelMetersFrontRight);
            writer.Write(telemetryData.SuspensionTravelMetersRearLeft);
            writer.Write(telemetryData.SuspensionTravelMetersRearRight);
            writer.Write(telemetryData.PositionX);
            writer.Write(telemetryData.PositionY);
            writer.Write(telemetryData.PositionZ);
            writer.Write(telemetryData.Speed);
            writer.Write(telemetryData.Power);
            writer.Write(telemetryData.Torque);
            writer.Write(telemetryData.TireTempFrontLeft);
            writer.Write(telemetryData.TireTempFrontRight);
            writer.Write(telemetryData.TireTempRearLeft);
            writer.Write(telemetryData.TireTempRearRight);
            writer.Write(telemetryData.Boost);
            writer.Write(telemetryData.Fuel);
            writer.Write(telemetryData.DistanceTraveled);
            writer.Write(telemetryData.BestLap);
            writer.Write(telemetryData.LastLap);
            writer.Write(telemetryData.CurrentLap);
            writer.Write(telemetryData.CurrentRaceTime);
            writer.Write(telemetryData.LapNumber);
            writer.Write(telemetryData.RacePosition);
            writer.Write(telemetryData.Accel);
            writer.Write(telemetryData.Brake);
            writer.Write(telemetryData.Clutch);
            writer.Write(telemetryData.HandBrake);
            writer.Write(telemetryData.Gear);
            writer.Write(telemetryData.Steer);
            writer.Write(telemetryData.NormalizedDrivingLine);
            writer.Write(telemetryData.NormalizedAIBrakeDifference);
            writer.Write(telemetryData.TireWearFrontLeft);
            writer.Write(telemetryData.TireWearFrontRight);
            writer.Write(telemetryData.TireWearRearLeft);
            writer.Write(telemetryData.TireWearRearRight);
            writer.Write(telemetryData.TrackOrdinal);
            writer.Write(telemetryData.CarOrdinal);
            writer.Write(telemetryData.CarClass);
            writer.Write(telemetryData.CarPerformanceIndex);
            writer.Write(telemetryData.DrivetrainType);
            writer.Write(telemetryData.NumCylinders);
            return stream.ToArray();
        }
    }

}
