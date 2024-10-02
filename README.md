# Insight.TelegramBot

[![Build & test](https://github.com/nazarovsa/Insight.TelegramBot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nazarovsa/Insight.TelegramBot/actions/workflows/dotnet.yml)
[![nuget version](https://img.shields.io/nuget/v/Insight.TelegramBot)](https://www.nuget.org/packages/Insight.TelegramBot/)
[![Nuget](https://img.shields.io/nuget/dt/Insight.TelegramBot?color=%2300000)](https://www.nuget.org/packages/Insight.TelegramBot/)

Insight.TelegramBot is a powerful and flexible SDK for implementing Telegram bot infrastructure in C#. It provides a set of tools and abstractions to simplify the development of Telegram bots, including update handling, state management, and easy integration with ASP.NET Core applications.

## Features

- Wrapper over [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) with additional functionality
- Support for both polling and webhook update reception methods
- Flexible update handling system with matchers and handlers
- State management for bot conversations
- Easy integration with dependency injection
- Support for various message types (text, photo, document, etc.)
- Extensible architecture for custom implementations

## Installation

Before installation you shoud add nuget source for Telegram.Bot package:

```
dotnet nuget add source https://pkgs.dev.azure.com/tgbots/Telegram.Bot/_packaging/release/nuget/v3/index.json -n Telegram.Bot
```

You can install Insight.TelegramBot via NuGet Package Manager:

```
dotnet add package Insight.TelegramBot
```

Depending on your needs, you may also want to install additional packages:

```
dotnet add package Insight.TelegramBot.Handling
dotnet add package Insight.TelegramBot.DependencyInjection
dotnet add package Insight.TelegramBot.DependencyInjection.Polling
dotnet add package Insight.TelegramBot.DependencyInjection.WebHook
```

## Quick Start

Here's a basic example of how to set up a simple Telegram bot using Insight.TelegramBot:

```csharp
using Insight.TelegramBot.DependencyInjection.Infrastructure;
using Insight.TelegramBot.DependencyInjection.Polling;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTelegramBot(builder => builder
            .WithTelegramBotClient(client => client
                .WithMicrosoftHttpClientFactory()
                .WithLifetime(ServiceLifetime.Singleton))
            .WithOptions(opt => opt.FromConfiguration(Configuration))
            .WithPolling(polling => polling
                .WithExceptionHandler<LoggingPollingExceptionHandler>()));
    }
}
```

## Update Handling

Insight.TelegramBot provides a flexible system for handling updates. You can create custom handlers and matchers to process different types of updates:

```csharp
public class StartMessageHandler : IMatchingUpdateHandler<StartMessageMatcher>
{
    public Task Handle(Update update, CancellationToken cancellationToken = default)
    {
        // Handle the /start command
        return Task.CompletedTask;
    }
}

public class StartMessageMatcher : TextEqualsUpdateMatcher
{
    public StartMessageMatcher()
    {
        Template = "/start";
    }
}
```

## WebHook vs Polling

Insight.TelegramBot supports both webhook and polling methods for receiving updates. You can choose the appropriate method based on your deployment environment and requirements.

### Webhook

```csharp
services.AddTelegramBot(builder => builder
    .WithTelegramBotClient(/* ... */)
    .WithOptions(/* ... */)
    .WithWebHook(webhook => webhook
        .WithDefaultUpdateController()));
```

### Polling

```csharp
services.AddTelegramBot(builder => builder
    .WithTelegramBotClient(/* ... */)
    .WithOptions(/* ... */)
    .WithPolling(polling => polling
        .WithExceptionHandler<LoggingPollingExceptionHandler>()));
```

## Advanced Usage

For more advanced usage, including custom update processors, state management, and complex message handling, please refer to the samples in the repository:

- [Simple Hosted Bot](https://github.com/nazarovsa/Insight.TelegramBot/tree/master/samples/Insight.TelegramBot.Samples.SimpleHostedBot)
- [WebHook Bot](https://github.com/nazarovsa/Insight.TelegramBot/tree/master/samples/Insight.TelegramBot.Samples.WebHookBot)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.