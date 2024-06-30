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
    /// LapTimespan
    /// </summary>
    public partial class LapTimespan : global::Avro.Specific.ISpecificRecord
    {
        /// <summary>
        /// StartTS
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("StartTS")]
        public long StartTS { get; set; }
        /// <summary>
        /// EndTS
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("EndTS")]
        public long EndTS { get; set; }
        /// <summary>
        /// Default constructor
        ///</summary>
        public LapTimespan()
        {
        }
    
        /// <summary>
        /// Constructor from Avro GenericRecord
        ///</summary>
        public LapTimespan(global::Avro.Generic.GenericRecord obj)
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
        "{\"name\": \"LapTimespan\", \"type\": \"record\", \"fields\": [{\"name\": \"StartTS\", \"type\":"+
        " \"long\", \"logicalType\": \"timestamp-millis\"}, {\"name\": \"EndTS\", \"type\": \"long\", \""+
        "logicalType\": \"timestamp-millis\"}], \"namespace\": \"ForzaMotorsport.Telemetry\"}");
    
        global::Avro.Schema global::Avro.Specific.ISpecificRecord.Schema => AvroSchema;
    
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
        /// Creates an object from the data
        /// </summary>
        /// <param name="data">The input data to convert</param>
        /// <param name="contentTypeString">The content type string of the desired encoding</param>
        /// <returns>The converted object</returns>
        public static LapTimespan? FromData(object? data, string? contentTypeString )
        {
            if ( data == null ) return null;
            if ( data is LapTimespan) return (LapTimespan)data;
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
                    var reader = new global::Avro.Generic.GenericDatumReader<global::Avro.Generic.GenericRecord>(LapTimespan.AvroSchema, LapTimespan.AvroSchema);
                    return new LapTimespan(reader.Read(null, new global::Avro.IO.BinaryDecoder(stream)));
                }
                if ( contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
                {
                    var reader = new global::Avro.Generic.GenericDatumReader<global::Avro.Generic.GenericRecord>(LapTimespan.AvroSchema, LapTimespan.AvroSchema);
                    return new LapTimespan(reader.Read(null, new global::Avro.IO.JsonDecoder(LapTimespan.AvroSchema, stream)));
                }
                #pragma warning restore CS8625
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
                else if (data is byte[])
                {
                    return System.Text.Json.JsonSerializer.Deserialize<LapTimespan>(new ReadOnlySpan<byte>((byte[])data));
                }
                else if (data is System.IO.Stream)
                {
                    return System.Text.Json.JsonSerializer.DeserializeAsync<LapTimespan>((System.IO.Stream)data).Result;
                }
            }
            throw new System.NotSupportedException($"Unsupported media type {contentType.MediaType}");
        }
        private class SpecificDatumWriter : global::Avro.Specific.SpecificDatumWriter<LapTimespan>
        {
            public SpecificDatumWriter() : base(LapTimespan.AvroSchema)
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
                var writer = new global::Avro.Specific.SpecificDatumWriter<LapTimespan>(LapTimespan.AvroSchema);
                var encoder = new global::Avro.IO.JsonEncoder(LapTimespan.AvroSchema, stream);
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
            (element.TryGetProperty("StartTS", out System.Text.Json.JsonElement StartTS) && (StartTS.ValueKind == System.Text.Json.JsonValueKind.Number)) &&
            (element.TryGetProperty("EndTS", out System.Text.Json.JsonElement EndTS) && (EndTS.ValueKind == System.Text.Json.JsonValueKind.Number)) &&
            true;
        }
    }
}