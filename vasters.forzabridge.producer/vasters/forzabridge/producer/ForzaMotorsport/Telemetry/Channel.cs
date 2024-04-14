#pragma warning disable CS8618
#pragma warning disable CS8603

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry
{
    public partial class Channel
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