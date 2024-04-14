#pragma warning disable CS8618
#pragma warning disable CS8603

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry
{
    public partial class LapSignal
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
        [JsonPropertyName("Timespan")]
        public global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapTimespan Timespan { get; set; }
    
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
    
        public static bool IsJsonMatch(System.Text.Json.JsonElement element)
        {
            return (element.TryGetProperty("LapId", out System.Text.Json.JsonElement LapId) && (LapId.ValueKind == System.Text.Json.JsonValueKind.String)) && 
                (!element.TryGetProperty("CarId", out System.Text.Json.JsonElement CarId) || (CarId.ValueKind == System.Text.Json.JsonValueKind.String) || CarId.ValueKind == System.Text.Json.JsonValueKind.Null) && 
                (!element.TryGetProperty("SessionId", out System.Text.Json.JsonElement SessionId) || (SessionId.ValueKind == System.Text.Json.JsonValueKind.String) || SessionId.ValueKind == System.Text.Json.JsonValueKind.Null) && 
                (element.TryGetProperty("Timespan", out System.Text.Json.JsonElement Timespan) && (global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapTimespan.IsJsonMatch(Timespan)));
        }
    }
}