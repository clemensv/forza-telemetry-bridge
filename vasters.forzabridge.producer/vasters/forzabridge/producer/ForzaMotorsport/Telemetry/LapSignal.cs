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
    /// <summary>
    /// LapSignal
    /// </summary>
    public partial class LapSignal : global::Avro.Specific.ISpecificRecord
    {
        /// <summary>
        /// The unique identifier of the lap
        /// </summary>
        [JsonPropertyName("LapId")]
        public string LapId { get; set; }
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
        /// <summary>
        /// Timespan
        /// </summary>
        [JsonPropertyName("Timespan")]
        public global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapTimespan Timespan { get; set; }
    
        /// <summary>
        /// Avro schema for this class
        /// </summary>
        public static global::Avro.Schema AvroSchema = global::Avro.Schema.Parse(
        "{\"name\": \"LapSignal\", \"type\": \"record\", \"namespace\": \"ForzaMotorsport.Telemetry\""+
        ", \"fields\": [{\"name\": \"LapId\", \"doc\": \"The unique identifier of the lap\", \"type\""+
        ": \"string\"}, {\"name\": \"CarId\", \"doc\": \"The unique identifier of the car\", \"type\""+
        ": [\"null\", \"string\"]}, {\"name\": \"SessionId\", \"doc\": \"The unique identifier of th"+
        "e session\", \"type\": [\"null\", \"string\"]}, {\"name\": \"Timespan\", \"type\": {\"name\": \""+
        "LapTimespan\", \"type\": \"record\", \"fields\": [{\"name\": \"StartTS\", \"type\": \"long\", \""+
        "logicalType\": \"timestamp-millis\"}, {\"name\": \"EndTS\", \"type\": \"long\", \"logicalTyp"+
        "e\": \"timestamp-millis\"}]}}]}");
    
        Schema global::Avro.Specific.ISpecificRecord.Schema => AvroSchema;
    
        object global::Avro.Specific.ISpecificRecord.Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return this.LapId;
                case 1: return this.CarId;
                case 2: return this.SessionId;
                case 3: return this.Timespan;
                default: throw new global::Avro.AvroRuntimeException($"Bad index {fieldPos} in Get()");
            }
        }
        void global::Avro.Specific.ISpecificRecord.Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: this.LapId = (string)fieldValue; break;
                case 1: this.CarId = (string?)fieldValue; break;
                case 2: this.SessionId = (string?)fieldValue; break;
                case 3: this.Timespan = (global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapTimespan)fieldValue; break;
                default: throw new global::Avro.AvroRuntimeException($"Bad index {fieldPos} in Put()");
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
                var writer = new Avro.Specific.SpecificDatumWriter<LapSignal>(LapSignal.AvroSchema);
                writer.Write(this, new Avro.IO.BinaryEncoder(stream));
                result = stream.ToArray();
            }
            else if (contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
            {
                var stream = new System.IO.MemoryStream();
                var writer = new Avro.Specific.SpecificDatumWriter<LapSignal>(LapSignal.AvroSchema);
                writer.Write(this, new Avro.IO.JsonEncoder(LapSignal.AvroSchema, stream));
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
        /// Creates an object from the data
        /// </summary>
        /// <param name="data">The input data to convert</param>
        /// <param name="contentTypeString">The content type string of the derired encoding</param>
        /// <returns>The converted object</returns>
        public static LapSignal FromData(object data, string contentTypeString)
        {
            if ( data is LapSignal) return (LapSignal)data;
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
                    var reader = new Avro.Specific.SpecificDatumReader<LapSignal>(LapSignal.AvroSchema, LapSignal.AvroSchema);
                    return reader.Read(new LapSignal(), new Avro.IO.BinaryDecoder(stream));
                }
                if ( contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/avro+json"))
                {
                    var reader = new Avro.Specific.SpecificDatumReader<LapSignal>(LapSignal.AvroSchema, LapSignal.AvroSchema);
                    return reader.Read(new LapSignal(), new Avro.IO.JsonDecoder(LapSignal.AvroSchema, stream));
                }
            }
            if ( contentType.MediaType.StartsWith(System.Net.Mime.MediaTypeNames.Application.Json))
            {
                if (data is System.Text.Json.JsonElement) 
                {
                    return System.Text.Json.JsonSerializer.Deserialize<LapSignal>((System.Text.Json.JsonElement)data);
                }
                else if ( data is string)
                {
                    return System.Text.Json.JsonSerializer.Deserialize<LapSignal>((string)data);
                }
                else if (data is System.BinaryData)
                {
                    return ((System.BinaryData)data).ToObjectFromJson<LapSignal>();
                }
            }
            throw new System.NotSupportedException($"Unsupported media type {contentType.MediaType}");
            
        }
    
        /// <summary>
        /// Checks if the JSON element matches the schema
        /// </summary>
        /// <param name="element">The JSON element to check</param>
        public static bool IsJsonMatch(System.Text.Json.JsonElement element)
        {
            return (element.TryGetProperty("LapId", out System.Text.Json.JsonElement LapId) && (LapId.ValueKind == System.Text.Json.JsonValueKind.String)) && 
                (!element.TryGetProperty("CarId", out System.Text.Json.JsonElement CarId) || (CarId.ValueKind == System.Text.Json.JsonValueKind.String) || CarId.ValueKind == System.Text.Json.JsonValueKind.Null) && 
                (!element.TryGetProperty("SessionId", out System.Text.Json.JsonElement SessionId) || (SessionId.ValueKind == System.Text.Json.JsonValueKind.String) || SessionId.ValueKind == System.Text.Json.JsonValueKind.Null) && 
                (element.TryGetProperty("Timespan", out System.Text.Json.JsonElement Timespan) && (global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapTimespan.IsJsonMatch(Timespan)));
        }
    }
}