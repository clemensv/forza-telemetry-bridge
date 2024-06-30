#pragma warning disable CS8618
#pragma warning disable CS8603

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry
{
    /// <summary>
    /// Channel
    /// </summary>
    public partial class Channel : global::Avro.Specific.ISpecificRecord
    {
        /// <summary>
        /// The unique identifier of the channel
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ChannelId")]
        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        public global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.ChannelType ChannelId { get; set; }
        /// <summary>
        /// The unique identifier of the car
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("CarId")]
        public string? CarId { get; set; }
        /// <summary>
        /// The unique identifier of the session
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("SessionId")]
        public string? SessionId { get; set; }
        /// <summary>
        /// LapId
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("LapId")]
        public string? LapId { get; set; }
        /// <summary>
        /// The number of samples in this batch
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("SampleCount")]
        public long SampleCount { get; set; }
        /// <summary>
        /// The frequency of the channel
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("Frequency")]
        public long Frequency { get; set; }
        /// <summary>
        /// Timespan
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("Timespan")]
        public global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.BatchTimespan Timespan { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("Data")]
        public List<double> Data { get; set; }
        /// <summary>
        /// Default constructor
        ///</summary>
        public Channel()
        {
        }
    
        /// <summary>
        /// Constructor from Avro GenericRecord
        ///</summary>
        public Channel(global::Avro.Generic.GenericRecord obj)
        {
            global::Avro.Specific.ISpecificRecord self = this;
            for (int i = 0; obj.Schema.Fields.Count > i; ++i)
            {
                self.Put(i, obj.GetValue(i));
            }
        }
    
    
        /// <summary>
        /// Avro schema for this class
        /// </summary>
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
        "amp-millis\"}], \"namespace\": \"ForzaMotorsport.Telemetry\"}}, {\"name\": \"Data\", \"typ"+
        "e\": {\"type\": \"array\", \"items\": \"double\"}}]}");
    
        global::Avro.Schema global::Avro.Specific.ISpecificRecord.Schema => AvroSchema;
    
        object global::Avro.Specific.ISpecificRecord.Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return (global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.ChannelType)this.ChannelId;
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
                case 0: this.ChannelId = fieldValue is global::Avro.Generic.GenericEnum?Enum.Parse<global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.ChannelType>(((global::Avro.Generic.GenericEnum)fieldValue).Value):(global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.ChannelType)fieldValue; break;
                case 1: this.CarId = (string?)fieldValue; break;
                case 2: this.SessionId = (string?)fieldValue; break;
                case 3: this.LapId = (string?)fieldValue; break;
                case 4: this.SampleCount = (long)fieldValue; break;
                case 5: this.Frequency = (long)fieldValue; break;
                case 6: this.Timespan = fieldValue is global::Avro.Generic.GenericRecord?new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.BatchTimespan((global::Avro.Generic.GenericRecord)fieldValue):(global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.BatchTimespan)fieldValue; break;
                case 7: this.Data = fieldValue is Object[]?((Object[])fieldValue).Select(x => (double)x).ToList():(List<double>)fieldValue; break;
                default: throw new global::Avro.AvroRuntimeException($"Bad index {fieldPos} in Put()");
            }
        }
    
        /// <summary>
        /// Creates an object from the data
        /// </summary>
        /// <param name="data">The input data to convert</param>
        /// <param name="contentTypeString">The content type string of the desired encoding</param>
        /// <returns>The converted object</returns>
        public static Channel? FromData(object? data, string? contentTypeString )
        {
            if ( data == null ) return null;
            if ( data is Channel) return (Channel)data;
            if ( contentTypeString == null ) contentTypeString = System.Net.Mime.MediaTypeNames.Application.Octet;
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
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    gzip.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    data = memoryStream.ToArray();
                }
            }
            if ( contentType.MediaType.StartsWith("avro/") || contentType.MediaType.StartsWith("application/vnd.apache.avro") )
            {
                var stream = data switch
                {
                    System.IO.Stream s => s, System.BinaryData bd => bd.ToStream(), byte[] bytes => new System.IO.MemoryStream(bytes),
                    _ => throw new NotSupportedException("Data is not of a supported type for conversion to Stream")
                };
                #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                if (contentType.MediaType.StartsWith("avro/binary") || contentType.MediaType.StartsWith("application/vnd.apache.avro+avro"))
                {
                    var reader = new global::Avro.Generic.GenericDatumReader<global::Avro.Generic.GenericRecord>(Channel.AvroSchema, Channel.AvroSchema);
                    return new Channel(reader.Read(null, new global::Avro.IO.BinaryDecoder(stream)));
                }
                if ( contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
                {
                    var reader = new global::Avro.Generic.GenericDatumReader<global::Avro.Generic.GenericRecord>(Channel.AvroSchema, Channel.AvroSchema);
                    return new Channel(reader.Read(null, new global::Avro.IO.JsonDecoder(Channel.AvroSchema, stream)));
                }
                #pragma warning restore CS8625
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
                else if (data is byte[])
                {
                    return System.Text.Json.JsonSerializer.Deserialize<Channel>(new ReadOnlySpan<byte>((byte[])data));
                }
                else if (data is System.IO.Stream)
                {
                    return System.Text.Json.JsonSerializer.DeserializeAsync<Channel>((System.IO.Stream)data).Result;
                }
            }
            throw new System.NotSupportedException($"Unsupported media type {contentType.MediaType}");
        }
        private class SpecificDatumWriter : global::Avro.Specific.SpecificDatumWriter<Channel>
        {
            public SpecificDatumWriter() : base(Channel.AvroSchema)
            {
            }
    
             protected override WriteItem ResolveEnum(global::Avro.EnumSchema es)
             {
                 return base.ResolveEnum(global::Avro.EnumSchema.Create(es.Name, es.Symbols, GetType().Assembly.GetName().Name+"."+es.Namespace, null, null, es.Documentation, es.Default));
             }
        }
        /// <summary>
        /// Converts the object to a byte array
        /// </summary>
        /// <param name="contentTypeString">The content type string of the desired encoding</param>
        /// <returns>The encoded data</returns>
        public byte[] ToByteArray(string contentTypeString)
        {
            var contentType = new System.Net.Mime.ContentType(contentTypeString);
            byte[]? result = null;
            if (contentType.MediaType.StartsWith("avro/binary") || contentType.MediaType.StartsWith("application/vnd.apache.avro+avro"))
            {
                var stream = new System.IO.MemoryStream();
                var writer = new SpecificDatumWriter();
                var encoder = new global::Avro.IO.BinaryEncoder(stream);
                writer.Write(this, encoder);
                encoder.Flush();
                result = stream.ToArray();
            }
            else if (contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
            {
                var stream = new System.IO.MemoryStream();
                var writer = new global::Avro.Specific.SpecificDatumWriter<Channel>(Channel.AvroSchema);
                var encoder = new global::Avro.IO.JsonEncoder(Channel.AvroSchema, stream);
                writer.Write(this, encoder);
                encoder.Flush();
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
        /// <summary>
        /// Checks if the JSON element matches the schema
        /// </summary>
        /// <param name="element">The JSON element to check</param>
        public static bool IsJsonMatch(System.Text.Json.JsonElement element)
        {
            return
            (element.TryGetProperty("ChannelId", out System.Text.Json.JsonElement ChannelId) && ((ChannelId.ValueKind == System.Text.Json.JsonValueKind.String && Enum.TryParse<global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.ChannelType>(ChannelId.GetString(), true, out _ )))) &&
            (!element.TryGetProperty("CarId", out System.Text.Json.JsonElement CarId) || (CarId.ValueKind == System.Text.Json.JsonValueKind.String) || CarId.ValueKind == System.Text.Json.JsonValueKind.Null) &&
            (!element.TryGetProperty("SessionId", out System.Text.Json.JsonElement SessionId) || (SessionId.ValueKind == System.Text.Json.JsonValueKind.String) || SessionId.ValueKind == System.Text.Json.JsonValueKind.Null) &&
            (!element.TryGetProperty("LapId", out System.Text.Json.JsonElement LapId) || (LapId.ValueKind == System.Text.Json.JsonValueKind.String) || LapId.ValueKind == System.Text.Json.JsonValueKind.Null) &&
            (element.TryGetProperty("SampleCount", out System.Text.Json.JsonElement SampleCount) && (SampleCount.ValueKind == System.Text.Json.JsonValueKind.Number)) &&
            (element.TryGetProperty("Frequency", out System.Text.Json.JsonElement Frequency) && (Frequency.ValueKind == System.Text.Json.JsonValueKind.Number)) &&
            (element.TryGetProperty("Timespan", out System.Text.Json.JsonElement Timespan) && (global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.BatchTimespan.IsJsonMatch(Timespan))) &&
            (element.TryGetProperty("Data", out System.Text.Json.JsonElement Data) && true ) &&
            true;
        }
    }
}