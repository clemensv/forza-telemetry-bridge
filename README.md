# ForzaBridge for Azure Event Hubs and Microsoft Fabric Event Streams

## Overview

ForzaBridge is a console application designed to capture and forward Forza
Motorsport telemetry data to Microsoft Azure Event Hubs or Microsoft Fabric
Event Streams. It utilizes UDP to listen for telemetry data sent from Forza
Motorsport games and forwards this data after processing and formatting into
cloud event streams.

## Features

- Customizable telemetry data capture over UDP.
- Support for various data encodings (JSON, Avro) and compressions.
- Direct integration with Azure Event Hubs.
- Configurable data forwarding rates and session handling.
- Real-time telemetry data processing and forwarding.

## Installation

To install ForzaBridge, ensure you have .NET 8.0 or higher installed on your
system. You can clone this repository to build the application yourself.


## Configuration

ForzaBridge uses command-line options for configuration. Below is a list of
available options:

### Required Configuration

- `-c|--connection <connectionString>`: Azure Event Hub or Fabric Event Stream
  connection string.

or

- `-n|--namespace <namespace>`: Azure Event Hub namespace.
- `-e|--eventhub <eventHubName>`: Azure Name of the Event Hub.

You must provide either the connection string or the namespace and event hub
name to connect to the Azure Event Hub. If you choose the -n/-e options, the
application will use the Azure CLI settings to authenticate.

### Optional Configuration

- - `-i|--ip <ipAddress>`: IP address to bind the UDP listener. Defaults to
  'any'.
- `-p|--port <port>`: Port to bind the UDP listener. Defaults to `5300`.
- `-x|--dataEncoding <encoding>`: Encoding of the data (Json, AvroBinary,
  AvroJson, JsonGzip, AvroBinaryGZip, AvroJsonGZip). Defaults to `Json`.
- `-v|--cloudEventEncoding <cloudEventEncoding>`: Cloud event encoding (Binary,
  JsonStructured, AvroStructured). Defaults to `Binary`.
- `-d|--data <dataMode>`: Data mode (Sled, Dash, or Automatic). Defaults to
  `Automatic`.
- `-r|--rate <dataRate>`: Data sending rate in Hz. Defaults to `1`.
- `-t|--tenant <tenantId>`: Tenant ID. Defaults to `default`.
- `-s|--session <sessionId>`: Session ID, defaults to a timestamp.
- `--car <carId>`: Car ID, defaults automatically based on telemetry data.

## Usage

To start ForzaBridge, use the following command format from your terminal:

```sh
dotnet run -- -c YourConnectionString
```

## Data Handling

ForzaBridge listens for UDP packets containing telemetry data, parses the data
based on the configured data mode, and sends it to the specified Azure Event
Hub. The application is capable of handling different data rates and encodings
to optimize performance and cost.

Review the included [xRegistry manifest](./xregistry/forza-telemetry.xreg.json) for
details on the telemetry events and data schemas.

### Data Encoding

ForzaBridge supports various data encodings for telemetry data. The following
encodings are available:

- `Json`: JSON encoding.
- `AvroBinary`: Avro binary encoding.
- `AvroJson`: Avro JSON encoding.
- `JsonGzip`: JSON encoding with GZIP compression.
- `AvroBinaryGZip`: Avro binary encoding with GZIP compression.
- `AvroJsonGZip`: Avro JSON encoding with GZIP compression.

Thje `Json` encoding is the default and recommended encoding for telemetry data.
It encodes the data in JSON format, whereby the data structures conform to the 
Avro schema defined in the [xRegistry manifest](./xregistry/forza-telemetry.xreg.json).

Different from the `AvroJson` encoding, the `Json` encoding does not follow the
union encoding rules of Apache Avro, which allows for more straightforward data
processing and serialization.

### Cloud Event Encoding Mode

ForzaBridge supports various cloud event encoding modes for telemetry data. The
following encoding modes are available:

- `Binary`: Binary mode. The data is sent in the event data and the CloudEvent
  attributes are applied as application properties on the Event Hub message.
- `JsonStructured`: JSON structured encoding. The event is sent as a self-contained
    JSON object with CloudEvent attributes and data.
- `AvroStructured`: Avro structured encoding. The event is sent as a self-contained
    Avro object with CloudEvent attributes and data.

The `Binary` encoding mode is the default and recommended encoding mode for
telemetry data.

## Support

For support, feature requests, or bug reporting, you can create issues on the
project's GitHub issues page.

## Contributing

Contributions are welcome via pull requests. Please fork the repository and
submit your contributions as pull requests.

## License

ForzaBridge is licensed under the MIT License. See the LICENSE file for more
details.
