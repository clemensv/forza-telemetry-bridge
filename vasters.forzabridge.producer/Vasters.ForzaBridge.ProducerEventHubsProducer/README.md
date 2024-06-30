
# C# Azure Event Hubs Event Factory for `ForzaMotorsport.Telemetry`

This is a C# library that provides a factory class for creating Azure Event Hubs
events for the `ForzaMotorsport.Telemetry` message group, along with a set of classes
for the event data.

Namespace: `Vasters.ForzaBridge.Producer.ForzaMotorsport`
Class Name: `ForzaMotorsport.TelemetryEventFactory`

## Methods and Properties



### CreateChannelEvent Method

Creates an `EventData` object for the `ForzaMotorsport.Telemetry.Channel` message.
#### Event Description

Channel Timeseries Event

#### Usage

```csharp
public static EventData CreateChannelEvent(Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.Channel data, string tenantid, string carId, string channelId, string contentType = "application/json+gzip", CloudEventFormatter? formatter = null);
```

#### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| `data` | `Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.Channel` | The data to include in the event |
| `tenantid` | `string` | URI template argument |
| `carId` | `string` | URI template argument |
| `channelId` | `string` | URI template argument |
| `contentType` | `string` | The content type of the event data. Defaults to `application/json+gzip` |
| `formatter` | `CloudEventFormatter` | The formatter to use for structured CloudEvents mode. Defaults to `null` (binary mode) |

### CreateLapSignalEvent Method

Creates an `EventData` object for the `ForzaMotorsport.Telemetry.LapSignal` message.
#### Event Description

LapSignal Event

#### Usage

```csharp
public static EventData CreateLapSignalEvent(Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapSignal data, string tenantid, string carId, string sessionId, string contentType = "application/json+gzip", CloudEventFormatter? formatter = null);
```

#### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| `data` | `Vasters.ForzaBridge.Producer.ForzaMotorsport.Telemetry.LapSignal` | The data to include in the event |
| `tenantid` | `string` | URI template argument |
| `carId` | `string` | URI template argument |
| `sessionId` | `string` | URI template argument |
| `contentType` | `string` | The content type of the event data. Defaults to `application/json+gzip` |
| `formatter` | `CloudEventFormatter` | The formatter to use for structured CloudEvents mode. Defaults to `null` (binary mode) |
