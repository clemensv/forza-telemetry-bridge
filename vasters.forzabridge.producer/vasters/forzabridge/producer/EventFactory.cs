
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using Azure.Messaging.EventHubs;

namespace Vasters.ForzaBridge.Producer.ForzaMotorsport
{
    public partial class TelemetryEventFactory
    {
        private const string cePrefix = "cloudEvents_";
        private const string applicationOctetStream = "application/octet-stream";
        private delegate byte[] Serialize<T>(T data, string contentType);
        private static CloudEventFormatter defaultFormatter = new JsonEventFormatter();

        private static EventData CreateEventData<T>(CloudEvent? cloudEvent, T data, string contentType, Serialize<T>? bodySerializer = null, CloudEventFormatter? formatter = null) where T : notnull
        {
            if (formatter == null)
            {
                bool isRawBytes = typeof(T) == typeof(byte[]);
                if (bodySerializer == null && !isRawBytes)
                {
                    throw new ArgumentNullException(nameof(bodySerializer));
                }
                var eventPayload = bodySerializer == null ? (byte[])(object)data : bodySerializer(data, contentType);
                EventData eventData = new EventData(eventPayload);
                eventData.ContentType = contentType;
                if (cloudEvent != null)
                {
                    foreach (var attr in cloudEvent.GetPopulatedAttributes())
                    {
                        eventData.Properties.Add(cePrefix + attr.Key.Name, attr.Value);
                    }
                }
                return eventData;
            }
            else
            {
                if (cloudEvent == null)
                {
                    throw new ArgumentNullException(nameof(cloudEvent));
                }
                if ( formatter is CloudNative.CloudEvents.SystemTextJson.JsonEventFormatter && 
                     new System.Net.Mime.ContentType(contentType).MediaType == System.Net.Mime.MediaTypeNames.Application.Json )
                {
                    cloudEvent.Data = data;
                }
                else
                {
                    bool isRawBytes = typeof(T) == typeof(byte[]);
                    if (bodySerializer == null && !isRawBytes)
                    {
                        throw new ArgumentNullException(nameof(bodySerializer));
                    }
                    cloudEvent.Data = bodySerializer == null ? data : bodySerializer(data, contentType);
                }
                var eventBody = formatter.EncodeStructuredModeMessage(cloudEvent, out var eventContentType);
                var eventData = new EventData(eventBody)
                {
                    ContentType = eventContentType.ToString()
                };
                return eventData;
            }
        }

        
        /// <summary>
        /// Channel Timeseries Event
        /// </summary>
        public static EventData CreateChannelEvent(global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.Channel data, string tenantid, string carId, string channelId, string contentType = "application/json+gzip", CloudEventFormatter? formatter = null)
        {
            
            Serialize<global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.Channel>? bodySerializer = (formatter != null)?null:(global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.Channel data, string contentType) => {
                return data.ToByteArray(contentType);
            };
            CloudEvent cloudEvent = new CloudEvent()
            {
                Type = $"ForzaMotorsport.Telemetry.Channel",
                Source = new Uri($"fza://{tenantid}/{carId}", UriKind.RelativeOrAbsolute),
                Subject = $"{channelId}",
                Time = DateTime.UtcNow,
                DataContentType = contentType,
            };

            return CreateEventData(cloudEvent, data, contentType, bodySerializer, formatter);
            
            
        }
        
        /// <summary>
        /// LapSignal Event
        /// </summary>
        public static EventData CreateLapSignalEvent(global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapSignal data, string tenantid, string carId, string sessionId, string contentType = "application/json+gzip", CloudEventFormatter? formatter = null)
        {
            
            Serialize<global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapSignal>? bodySerializer = (formatter != null)?null:(global::Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapSignal data, string contentType) => {
                return data.ToByteArray(contentType);
            };
            CloudEvent cloudEvent = new CloudEvent()
            {
                Type = $"ForzaMotorsport.Telemetry.LapSignal",
                Source = new Uri($"fza://{tenantid}/{carId}", UriKind.RelativeOrAbsolute),
                Subject = $"{sessionId}",
                Time = DateTime.UtcNow,
                DataContentType = contentType,
            };

            return CreateEventData(cloudEvent, data, contentType, bodySerializer, formatter);
            
            
        }
        
    }
}
