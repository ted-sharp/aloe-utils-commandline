# Aloe.Utils.CommandLine

A lightweight utility for flexible preprocessing of command-line arguments. It supports boolean option completion and concatenated short options with values, making your application's configuration more concise and robust when used with .NET's `IConfigurationBuilder.AddCommandLine(...)`.

## Key Features

* Completes standalone options like --flag to true
* Splits concatenated options like -uadmin to -u admin
* Zero-dependency lightweight utility
* Perfect for integration with DI / IConfiguration / appsettings.json

## Requirements

* .NET 9 or later
* Compatible with Microsoft.Extensions.Configuration

## Installation

```cmd
dotnet add package Aloe.Utils.CommandLine
```

## Usage Example

```csharp
using Aloe.Utils.CommandLine;

var processedArgs = ArgsHelper.PreprocessArgs(
    args,
    flagArgs: new[] { "--debug", "--standalone" },
    shortArgs: new[] { "-u", "-p" }
);
```

Input example:
```cmd
myapp.exe -uadmin --debug
```

Output result:
```csharp
new[] { "-u", "admin", "--debug", "true" }
```
