#pragma warning disable CS8618
#pragma warning disable CS8603

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry
{
    public partial class BatchTimespan
    {
        [JsonPropertyName("StartTS")]
        public long StartTS { get; set; }
        [JsonPropertyName("EndTS")]
        public long EndTS { get; set; }
    
        public byte[] ToByteArray(string contentTypeString)
        {
            var contentType = new System.Net.Mime.ContentType(contentTypeString);
            byte[]? result = null;
            
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
    
        public static bool IsJsonMatch(System.Text.Json.JsonElement element)
        {
            return (element.TryGetProperty("StartTS", out System.Text.Json.JsonElement StartTS) && (StartTS.ValueKind == System.Text.Json.JsonValueKind.Number)) && 
                (element.TryGetProperty("EndTS", out System.Text.Json.JsonElement EndTS) && (EndTS.ValueKind == System.Text.Json.JsonValueKind.Number));
        }
    }
}