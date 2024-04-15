#pragma warning disable CS8618
#pragma warning disable CS8603

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avro;
using Avro.Specific;

namespace Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry
{
    public partial class Channel : global::Avro.Specific.ISpecificRecord
    {
        /// <summary>
        /// The unique identifier of the channel
        /// </summary>
        [JsonPropertyName("ChannelId")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.ChannelType ChannelId { get; set; }
        /// <summary>
        /// The unique identifier of the car
        /// </summary>
        [JsonPropertyName("CarId")]
        public string? CarId { get; set; }
        /// <summary>
        /// The unique identifier of the session
        /// </summary>
        [JsonPropertyName("SessionId")]
        public string? SessionId { get; set; }
        [JsonPropertyName("LapId")]
        public string? LapId { get; set; }
        /// <summary>
        /// The number of samples in this batch
        /// </summary>
        [JsonPropertyName("SampleCount")]
        public long SampleCount { get; set; }
        /// <summary>
        /// The frequency of the channel
        /// </summary>
        [JsonPropertyName("Frequency")]
        public long Frequency { get; set; }
        [JsonPropertyName("Timespan")]
        public global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.BatchTimespan Timespan { get; set; }
        [JsonPropertyName("Data")]
        public List<double> Data { get; set; }
    
        public static global::Avro.Schema AvroSchema = global::Avro.Schema.Parse(
        "{\"name\": \"Channel\", \"type\": \"record\", \"namespace\": \"ForzaMotorsport.Telemetry\", "+
        "\"fields\": [{\"name\": \"ChannelId\", \"doc\": \"The unique identifier of the channel\", "+
        "\"type\": {\"name\": \"ChannelType\", \"type\": \"enum\", \"symbols\": [\"EngineMaxRpm\", \"Eng"+
        "ineIdleRpm\", \"CurrentEngineRpm\", \"AccelerationX\", \"AccelerationY\", \"Acceleration"+
        "Z\", \"VelocityX\", \"VelocityY\", \"VelocityZ\", \"AngularVelocityX\", \"AngularVelocityY"+
        "\", \"AngularVelocityZ\", \"Yaw\", \"Pitch\", \"Roll\", \"NormalizedSuspensionTravelFrontL"+
        "eft\", \"NormalizedSuspensionTravelFrontRight\", \"NormalizedSuspensionTravelRearLef"+
        "t\", \"NormalizedSuspensionTravelRearRight\", \"TireSlipRatioFrontLeft\", \"TireSlipRa"+
        "tioFrontRight\", \"TireSlipRatioRearLeft\", \"TireSlipRatioRearRight\", \"WheelRotatio"+
        "nSpeedFrontLeft\", \"WheelRotationSpeedFrontRight\", \"WheelRotationSpeedRearLeft\", "+
        "\"WheelRotationSpeedRearRight\", \"WheelOnRumbleStripFrontLeft\", \"WheelOnRumbleStri"+
        "pFrontRight\", \"WheelOnRumbleStripRearLeft\", \"WheelOnRumbleStripRearRight\", \"Whee"+
        "lInPuddleDepthFrontLeft\", \"WheelInPuddleDepthFrontRight\", \"WheelInPuddleDepthRea"+
        "rLeft\", \"WheelInPuddleDepthRearRight\", \"SurfaceRumbleFrontLeft\", \"SurfaceRumbleF"+
        "rontRight\", \"SurfaceRumbleRearLeft\", \"SurfaceRumbleRearRight\", \"TireSlipAngleFro"+
        "ntLeft\", \"TireSlipAngleFrontRight\", \"TireSlipAngleRearLeft\", \"TireSlipAngleRearR"+
        "ight\", \"TireCombinedSlipFrontLeft\", \"TireCombinedSlipFrontRight\", \"TireCombinedS"+
        "lipRearLeft\", \"TireCombinedSlipRearRight\", \"SuspensionTravelMetersFrontLeft\", \"S"+
        "uspensionTravelMetersFrontRight\", \"SuspensionTravelMetersRearLeft\", \"SuspensionT"+
        "ravelMetersRearRight\", \"PositionX\", \"PositionY\", \"PositionZ\", \"Speed\", \"Power\", "+
        "\"Torque\", \"TireTempFrontLeft\", \"TireTempFrontRight\", \"TireTempRearLeft\", \"TireTe"+
        "mpRearRight\", \"Boost\", \"Fuel\", \"DistanceTraveled\", \"RacePosition\", \"Accel\", \"Bra"+
        "ke\", \"Clutch\", \"HandBrake\", \"Gear\", \"Steer\", \"NormalizedDrivingLine\", \"Normalize"+
        "dAIBrakeDifference\", \"TireWearFrontLeft\", \"TireWearFrontRight\", \"TireWearRearLef"+
        "t\", \"TireWearRearRight\"]}}, {\"name\": \"CarId\", \"doc\": \"The unique identifier of t"+
        "he car\", \"type\": [\"null\", \"string\"]}, {\"name\": \"SessionId\", \"doc\": \"The unique i"+
        "dentifier of the session\", \"type\": [\"null\", \"string\"]}, {\"name\": \"LapId\", \"type\""+
        ": [\"null\", \"string\"]}, {\"name\": \"SampleCount\", \"doc\": \"The number of samples in "+
        "this batch\", \"type\": \"long\"}, {\"name\": \"Frequency\", \"doc\": \"The frequency of the"+
        " channel\", \"type\": \"long\"}, {\"name\": \"Timespan\", \"type\": {\"name\": \"BatchTimespan"+
        "\", \"type\": \"record\", \"fields\": [{\"name\": \"StartTS\", \"type\": \"long\", \"logicalType"+
        "\": \"timestamp-millis\"}, {\"name\": \"EndTS\", \"type\": \"long\", \"logicalType\": \"timest"+
        "amp-millis\"}]}}, {\"name\": \"Data\", \"type\": {\"type\": \"array\", \"items\": \"double\"}}]"+
        "}");
    
        Schema global::Avro.Specific.ISpecificRecord.Schema => AvroSchema;
    
        object global::Avro.Specific.ISpecificRecord.Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return this.ChannelId;
                case 1: return this.CarId;
                case 2: return this.SessionId;
                case 3: return this.LapId;
                case 4: return this.SampleCount;
                case 5: return this.Frequency;
                case 6: return this.Timespan;
                case 7: return this.Data;
                default: throw new global::Avro.AvroRuntimeException($"Bad index {fieldPos} in Get()");
            }
        }
        void global::Avro.Specific.ISpecificRecord.Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: this.ChannelId = (global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.ChannelType)fieldValue; break;
                case 1: this.CarId = (string?)fieldValue; break;
                case 2: this.SessionId = (string?)fieldValue; break;
                case 3: this.LapId = (string?)fieldValue; break;
                case 4: this.SampleCount = (long)fieldValue; break;
                case 5: this.Frequency = (long)fieldValue; break;
                case 6: this.Timespan = (global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.BatchTimespan)fieldValue; break;
                case 7: this.Data = (List<double>)fieldValue; break;
                default: throw new global::Avro.AvroRuntimeException($"Bad index {fieldPos} in Put()");
            }
        }
    
    
        public byte[] ToByteArray(string contentTypeString)
        {
            var contentType = new System.Net.Mime.ContentType(contentTypeString);
            byte[]? result = null;
            
            if (contentType.MediaType.StartsWith("avro/binary") || contentType.MediaType.StartsWith("application/vnd.apache.avro+avro"))
            {
                var stream = new System.IO.MemoryStream();
                var writer = new Avro.Specific.SpecificDatumWriter<Channel>(Channel.AvroSchema);
                writer.Write(this, new Avro.IO.BinaryEncoder(stream));
                result = stream.ToArray();
            }
            else if (contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
            {
                var stream = new System.IO.MemoryStream();
                var writer = new Avro.Specific.SpecificDatumWriter<Channel>(Channel.AvroSchema);
                writer.Write(this, new Avro.IO.JsonEncoder(Channel.AvroSchema, stream));
                result = stream.ToArray();
            }
            if (contentType.MediaType.StartsWith(System.Net.Mime.MediaTypeNames.Application.Json))
            {
                result = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(this);
            }
            if (result != null && contentType.MediaType.EndsWith("+gzip"))
            {
                var stream = new System.IO.MemoryStream();
                using (var gzip = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Compress))
                {
                    gzip.Write(result, 0, result.Length);
                }
                result = stream.ToArray();
            }
            return ( result != null ) ? result : throw new System.NotSupportedException($"Unsupported media type {contentType.MediaType}");
            
        }
    
        public static Channel FromData(object data, string contentTypeString)
        {
            if ( data is Channel) return (Channel)data;
            var contentType = new System.Net.Mime.ContentType(contentTypeString);
            if ( contentType.MediaType.EndsWith("+gzip"))
            {
                var stream = data switch
                {
                    System.IO.Stream s => s, System.BinaryData bd => bd.ToStream(), byte[] bytes => new System.IO.MemoryStream(bytes),
                    _ => throw new NotSupportedException("Data is not of a supported type for gzip decompression")
                };
                using (var gzip = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                {
                    data = new System.IO.MemoryStream();
                    gzip.CopyTo((System.IO.MemoryStream)data);
                }
            }
            
            if ( contentType.MediaType.StartsWith("avro/") || contentType.MediaType.StartsWith("application/vnd.apache.avro") )
            {
                var stream = data switch
                {
                    System.IO.Stream s => s, System.BinaryData bd => bd.ToStream(), byte[] bytes => new System.IO.MemoryStream(bytes),
                    _ => throw new NotSupportedException("Data is not of a supported type for conversion to Stream")
                };
                if (contentType.MediaType.StartsWith("avro/binary") || contentType.MediaType.StartsWith("application/vnd.apache.avro+avro"))
                {
                    var reader = new Avro.Specific.SpecificDatumReader<Channel>(Channel.AvroSchema, Channel.AvroSchema);
                    return reader.Read(new Channel(), new Avro.IO.BinaryDecoder(stream));
                }
                if ( contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/avro+json"))
                {
                    var reader = new Avro.Specific.SpecificDatumReader<Channel>(Channel.AvroSchema, Channel.AvroSchema);
                    return reader.Read(new Channel(), new Avro.IO.JsonDecoder(Channel.AvroSchema, stream));
                }
            }
            if ( contentType.MediaType.StartsWith(System.Net.Mime.MediaTypeNames.Application.Json))
            {
                if (data is System.Text.Json.JsonElement) 
                {
                    return System.Text.Json.JsonSerializer.Deserialize<Channel>((System.Text.Json.JsonElement)data);
                }
                else if ( data is string)
                {
                    return System.Text.Json.JsonSerializer.Deserialize<Channel>((string)data);
                }
                else if (data is System.BinaryData)
                {
                    return ((System.BinaryData)data).ToObjectFromJson<Channel>();
                }
            }
            throw new System.NotSupportedException($"Unsupported media type {contentType.MediaType}");
            
        }
    
        public static bool IsJsonMatch(System.Text.Json.JsonElement element)
        {
            return (element.TryGetProperty("ChannelId", out System.Text.Json.JsonElement ChannelId) && ((ChannelId.ValueKind == System.Text.Json.JsonValueKind.String && Enum.TryParse<global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.ChannelType>(ChannelId.GetString(), true, out _ )))) && 
                (!element.TryGetProperty("CarId", out System.Text.Json.JsonElement CarId) || (CarId.ValueKind == System.Text.Json.JsonValueKind.String) || CarId.ValueKind == System.Text.Json.JsonValueKind.Null) && 
                (!element.TryGetProperty("SessionId", out System.Text.Json.JsonElement SessionId) || (SessionId.ValueKind == System.Text.Json.JsonValueKind.String) || SessionId.ValueKind == System.Text.Json.JsonValueKind.Null) && 
                (!element.TryGetProperty("LapId", out System.Text.Json.JsonElement LapId) || (LapId.ValueKind == System.Text.Json.JsonValueKind.String) || LapId.ValueKind == System.Text.Json.JsonValueKind.Null) && 
                (element.TryGetProperty("SampleCount", out System.Text.Json.JsonElement SampleCount) && (SampleCount.ValueKind == System.Text.Json.JsonValueKind.Number)) && 
                (element.TryGetProperty("Frequency", out System.Text.Json.JsonElement Frequency) && (Frequency.ValueKind == System.Text.Json.JsonValueKind.Number)) && 
                (element.TryGetProperty("Timespan", out System.Text.Json.JsonElement Timespan) && (global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.BatchTimespan.IsJsonMatch(Timespan))) && 
                (element.TryGetProperty("Data", out System.Text.Json.JsonElement Data) && true );
        }
    }
}