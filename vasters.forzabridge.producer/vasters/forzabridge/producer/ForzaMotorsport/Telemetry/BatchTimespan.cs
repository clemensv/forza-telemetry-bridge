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
    /// BatchTimespan
    /// </summary>
    public partial class BatchTimespan : global::Avro.Specific.ISpecificRecord
    {
        /// <summary>
        /// StartTS
        /// </summary>
        [JsonPropertyName("StartTS")]
        public long StartTS { get; set; }
        /// <summary>
        /// EndTS
        /// </summary>
        [JsonPropertyName("EndTS")]
        public long EndTS { get; set; }
    
        /// <summary>
        /// Avro schema for this class
        /// </summary>
        public static global::Avro.Schema AvroSchema = global::Avro.Schema.Parse(
        "{\"name\": \"BatchTimespan\", \"type\": \"record\", \"fields\": [{\"name\": \"StartTS\", \"type"+
        "\": \"long\", \"logicalType\": \"timestamp-millis\"}, {\"name\": \"EndTS\", \"type\": \"long\","+
        " \"logicalType\": \"timestamp-millis\"}]}");
    
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
                var writer = new Avro.Specific.SpecificDatumWriter<BatchTimespan>(BatchTimespan.AvroSchema);
                writer.Write(this, new Avro.IO.BinaryEncoder(stream));
                result = stream.ToArray();
            }
            else if (contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
            {
                var stream = new System.IO.MemoryStream();
                var writer = new Avro.Specific.SpecificDatumWriter<BatchTimespan>(BatchTimespan.AvroSchema);
                writer.Write(this, new Avro.IO.JsonEncoder(BatchTimespan.AvroSchema, stream));
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
        public static BatchTimespan FromData(object data, string contentTypeString)
        {
            if ( data is BatchTimespan) return (BatchTimespan)data;
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
                    var reader = new Avro.Specific.SpecificDatumReader<BatchTimespan>(BatchTimespan.AvroSchema, BatchTimespan.AvroSchema);
                    return reader.Read(new BatchTimespan(), new Avro.IO.BinaryDecoder(stream));
                }
                if ( contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/avro+json"))
                {
                    var reader = new Avro.Specific.SpecificDatumReader<BatchTimespan>(BatchTimespan.AvroSchema, BatchTimespan.AvroSchema);
                    return reader.Read(new BatchTimespan(), new Avro.IO.JsonDecoder(BatchTimespan.AvroSchema, stream));
                }
            }
            if ( contentType.MediaType.StartsWith(System.Net.Mime.MediaTypeNames.Application.Json))
            {
                if (data is System.Text.Json.JsonElement) 
                {
                    return System.Text.Json.JsonSerializer.Deserialize<BatchTimespan>((System.Text.Json.JsonElement)data);
                }
                else if ( data is string)
                {
                    return System.Text.Json.JsonSerializer.Deserialize<BatchTimespan>((string)data);
                }
                else if (data is System.BinaryData)
                {
                    return ((System.BinaryData)data).ToObjectFromJson<BatchTimespan>();
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
            return (element.TryGetProperty("StartTS", out System.Text.Json.JsonElement StartTS) && (StartTS.ValueKind == System.Text.Json.JsonValueKind.Number)) && 
                (element.TryGetProperty("EndTS", out System.Text.Json.JsonElement EndTS) && (EndTS.ValueKind == System.Text.Json.JsonValueKind.Number));
        }
    }
}