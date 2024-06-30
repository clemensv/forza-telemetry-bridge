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
    /// LapSignal
    /// </summary>
    public partial class LapSignal : global::Avro.Specific.ISpecificRecord
    {
        /// <summary>
        /// The unique identifier of the lap
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("LapId")]
        public string LapId { get; set; }
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
        /// Timespan
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("Timespan")]
        public global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.LapTimespan Timespan { get; set; }
        /// <summary>
        /// Default constructor
        ///</summary>
        public LapSignal()
        {
        }
    
        /// <summary>
        /// Constructor from Avro GenericRecord
        ///</summary>
        public LapSignal(global::Avro.Generic.GenericRecord obj)
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
        "{\"name\": \"LapSignal\", \"type\": \"record\", \"namespace\": \"ForzaMotorsport.Telemetry\""+
        ", \"fields\": [{\"name\": \"LapId\", \"doc\": \"The unique identifier of the lap\", \"type\""+
        ": \"string\"}, {\"name\": \"CarId\", \"doc\": \"The unique identifier of the car\", \"type\""+
        ": [\"null\", \"string\"]}, {\"name\": \"SessionId\", \"doc\": \"The unique identifier of th"+
        "e session\", \"type\": [\"null\", \"string\"]}, {\"name\": \"Timespan\", \"type\": {\"name\": \""+
        "LapTimespan\", \"type\": \"record\", \"fields\": [{\"name\": \"StartTS\", \"type\": \"long\", \""+
        "logicalType\": \"timestamp-millis\"}, {\"name\": \"EndTS\", \"type\": \"long\", \"logicalTyp"+
        "e\": \"timestamp-millis\"}], \"namespace\": \"ForzaMotorsport.Telemetry\"}}]}");
    
        global::Avro.Schema global::Avro.Specific.ISpecificRecord.Schema => AvroSchema;
    
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
                case 3: this.Timespan = fieldValue is global::Avro.Generic.GenericRecord?new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.LapTimespan((global::Avro.Generic.GenericRecord)fieldValue):(global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.LapTimespan)fieldValue; break;
                default: throw new global::Avro.AvroRuntimeException($"Bad index {fieldPos} in Put()");
            }
        }
    
        /// <summary>
        /// Creates an object from the data
        /// </summary>
        /// <param name="data">The input data to convert</param>
        /// <param name="contentTypeString">The content type string of the desired encoding</param>
        /// <returns>The converted object</returns>
        public static LapSignal? FromData(object? data, string? contentTypeString )
        {
            if ( data == null ) return null;
            if ( data is LapSignal) return (LapSignal)data;
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
                    var reader = new global::Avro.Generic.GenericDatumReader<global::Avro.Generic.GenericRecord>(LapSignal.AvroSchema, LapSignal.AvroSchema);
                    return new LapSignal(reader.Read(null, new global::Avro.IO.BinaryDecoder(stream)));
                }
                if ( contentType.MediaType.StartsWith("avro/json") || contentType.MediaType.StartsWith("application/vnd.apache.avro+json"))
                {
                    var reader = new global::Avro.Generic.GenericDatumReader<global::Avro.Generic.GenericRecord>(LapSignal.AvroSchema, LapSignal.AvroSchema);
                    return new LapSignal(reader.Read(null, new global::Avro.IO.JsonDecoder(LapSignal.AvroSchema, stream)));
                }
                #pragma warning restore CS8625
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
                else if (data is byte[])
                {
                    return System.Text.Json.JsonSerializer.Deserialize<LapSignal>(new ReadOnlySpan<byte>((byte[])data));
                }
                else if (data is System.IO.Stream)
                {
                    return System.Text.Json.JsonSerializer.DeserializeAsync<LapSignal>((System.IO.Stream)data).Result;
                }
            }
            throw new System.NotSupportedException($"Unsupported media type {contentType.MediaType}");
        }
        private class SpecificDatumWriter : global::Avro.Specific.SpecificDatumWriter<LapSignal>
        {
            public SpecificDatumWriter() : base(LapSignal.AvroSchema)
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
                var writer = new global::Avro.Specific.SpecificDatumWriter<LapSignal>(LapSignal.AvroSchema);
                var encoder = new global::Avro.IO.JsonEncoder(LapSignal.AvroSchema, stream);
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
            (element.TryGetProperty("LapId", out System.Text.Json.JsonElement LapId) && (LapId.ValueKind == System.Text.Json.JsonValueKind.String)) &&
            (!element.TryGetProperty("CarId", out System.Text.Json.JsonElement CarId) || (CarId.ValueKind == System.Text.Json.JsonValueKind.String) || CarId.ValueKind == System.Text.Json.JsonValueKind.Null) &&
            (!element.TryGetProperty("SessionId", out System.Text.Json.JsonElement SessionId) || (SessionId.ValueKind == System.Text.Json.JsonValueKind.String) || SessionId.ValueKind == System.Text.Json.JsonValueKind.Null) &&
            (element.TryGetProperty("Timespan", out System.Text.Json.JsonElement Timespan) && (global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.LapTimespan.IsJsonMatch(Timespan))) &&
            true;
        }
    }
}