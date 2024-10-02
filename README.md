# Insight.TelegramBot
[![Build & test](https://github.com/nazarovsa/Insight.TelegramBot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nazarovsa/Insight.TelegramBot/actions/workflows/dotnet.yml)
[![nuget version](https://img.shields.io/nuget/v/Insight.TelegramBot)](https://www.nuget.org/packages/Insight.TelegramBot/)
[![Nuget](https://img.shields.io/nuget/dt/Insight.TelegramBot?color=%2300000)](https://www.nuget.org/packages/Insight.TelegramBot/)

Simple SDK to implement telegram bot infrastructure. It includes tools to work with state in your bot and web extensions to simplify bot implementation at your Startup.cs

## Insight.TelegramBot
Includes wrapper over [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) and some additional things to simplify usage.

## Insight.TelegramBot.Handling

In progress... To fast start check [HandlingExtensionsTests](https://github.com/nazarovsa/Insight.TelegramBot/blob/master/tests/Insight.TelegramBot.Tests/HandlingExtensionsTests.cs)...

## Insight.TelegramBot.Hosting.DependencyInjection
[![nuget version](https://img.shields.io/nuget/v/Insight.TelegramBot.Hosting.DependencyInjection)](https://www.nuget.org/packages/Insight.TelegramBot.Hosting.DependencyInjection/)
[![Nuget](https://img.shields.io/nuget/dt/Insight.TelegramBot.Hosting.DependencyInjection?color=%2300000)](https://www.nuget.org/packages/Insight.TelegramBot.Hosting.DependencyInjection/)

Extensions to simplify bot registration.

Samples
------------------------
You can explore next samples:
* [Insight.TelegramBot.Samples.SimpleHostedBot](https://github.com/nazarovsa/Insight.TelegramBot/tree/master/samples/Insight.TelegramBot.Samples.SimpleHostedBot) - Simple polling bot
* [Insight.TelegramBot.Samples.WebHookBot](https://github.com/nazarovsa/Insight.TelegramBot/tree/master/samples/Insight.TelegramBot.Samples.WebHookBot) - Simple webhook bot
