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
    public partial class LapTimespan : global::Avro.Specific.ISpecificRecord
    {
        [JsonPropertyName("StartTS")]
        public long StartTS { get; set; }
        [JsonPropertyName("EndTS")]
        public long EndTS { get; set; }
    
        public static global::Avro.Schema AvroSchema = global::Avro.Schema.Parse(
        "{\"name\": \"LapTimespan\", \"type\": \"record\", \"fields\": [{\"name\": \"StartTS\", \"type\":"+
        " \"long\", \"logicalType\": \"timestamp-millis\"}, {\"name\": \"EndTS\", \"type\": \"long\", \""+
        "logicalType\": \"timestamp-millis\"}]}");
    
        Schema global::Avro.Specific.ISpecificRecord.Schema => AvroSchema;
    
        object global::Avro.Specific.ISpecificRecord.Get(int fieldPos)
        {
            switch (fieldPos)
            {
                case 0: return this.StartTS;
                case 1: return this.EndTS;
                default: throw new global::Avro.AvroRuntimeException($"Bad index {fieldPos} in Get()");
            }
        }
        void global::Avro.Specific.ISpecificRecord.Put(int fieldPos, object fieldValue)
        {
            switch (fieldPos)
            {
                case 0: this.StartTS = (long)fieldValue; break;
                case 1: this.EndTS = (long)fieldValue; break;
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
                var writer = new Avro.Specific.SpecificDatumWriter<LapTimespan>(LapTimespan.AvroSchema);
                writer.Write(this, new Avro.IO.BinaryEncoder(stream));
                result = stream.ToArray();
            }
            else if (contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
            {
                var stream = new System.IO.MemoryStream();
                var writer = new Avro.Specific.SpecificDatumWriter<LapTimespan>(LapTimespan.AvroSchema);
                writer.Write(this, new Avro.IO.JsonEncoder(LapTimespan.AvroSchema, stream));
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
    
        public static LapTimespan FromData(object data, string contentTypeString)
        {
            if ( data is LapTimespan) return (LapTimespan)data;
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
                    var reader = new Avro.Specific.SpecificDatumReader<LapTimespan>(LapTimespan.AvroSchema, LapTimespan.AvroSchema);
                    return reader.Read(new LapTimespan(), new Avro.IO.BinaryDecoder(stream));
                }
                if ( contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/avro+json"))
                {
                    var reader = new Avro.Specific.SpecificDatumReader<LapTimespan>(LapTimespan.AvroSchema, LapTimespan.AvroSchema);
                    return reader.Read(new LapTimespan(), new Avro.IO.JsonDecoder(LapTimespan.AvroSchema, stream));
                }
            }
            if ( contentType.MediaType.StartsWith(System.Net.Mime.MediaTypeNames.Application.Json))
            {
                if (data is System.Text.Json.JsonElement) 
                {
                    return System.Text.Json.JsonSerializer.Deserialize<LapTimespan>((System.Text.Json.JsonElement)data);
                }
                else if ( data is string)
                {
                    return System.Text.Json.JsonSerializer.Deserialize<LapTimespan>((string)data);
                }
                else if (data is System.BinaryData)
                {
                    return ((System.BinaryData)data).ToObjectFromJson<LapTimespan>();
                }
            }
            throw new System.NotSupportedException($"Unsupported media type {contentType.MediaType}");
            
        }
    
        public static bool IsJsonMatch(System.Text.Json.JsonElement element)
        {
            return (element.TryGetProperty("StartTS", out System.Text.Json.JsonElement StartTS) && (StartTS.ValueKind == System.Text.Json.JsonValueKind.Number)) && 
                (element.TryGetProperty("EndTS", out System.Text.Json.JsonElement EndTS) && (EndTS.ValueKind == System.Text.Json.JsonValueKind.Number));
        }
    }
}