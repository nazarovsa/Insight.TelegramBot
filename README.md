Insight.TelegramBot
========================
[![Build & test](https://github.com/nazarovsa/Insight.TelegramBot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nazarovsa/Insight.TelegramBot/actions/workflows/dotnet.yml)
[![nuget version](https://img.shields.io/nuget/v/Insight.TelegramBot)](https://www.nuget.org/packages/Insight.TelegramBot/)
[![Nuget](https://img.shields.io/nuget/dt/Insight.TelegramBot?color=%2300000)](https://www.nuget.org/packages/Insight.TelegramBot/)

Simple SDK to implement telegram bot infrastructure. It includes tools to work with state in your bot and web extensions to simplify bot implementation at your Startup.cs

Insight.TelegramBot.State
------------------------
[![nuget version](https://img.shields.io/nuget/v/Insight.TelegramBot.State)](https://www.nuget.org/packages/Insight.TelegramBot.State/)
[![Nuget](https://img.shields.io/nuget/dt/Insight.TelegramBot.State?color=%2300000)](https://www.nuget.org/packages/Insight.TelegramBot.State/)

Includes abstractions to implement state machine for your bot. You can see working example at [tests](https://github.com/InsightAppDev/Insight.TelegramBot/blob/master/tests/Insight.TelegramBot.State.Tests/StateMachineTest.cs). 
StateMachine is abstract class that uses [Stateless](https://www.nuget.org/packages/stateless) library to work with state, so you should know a little about how to use it to configure your state machine.

Insight.TelegramBot.Web
------------------------
[![nuget version](https://img.shields.io/nuget/v/Insight.TelegramBot.Web)](https://www.nuget.org/packages/Insight.TelegramBot.Web/)
[![Nuget](https://img.shields.io/nuget/dt/Insight.TelegramBot.Web?color=%2300000)](https://www.nuget.org/packages/Insight.TelegramBot.Web/)

Extensions to simplify bot implementation at Startup.cs


Samples
------------------------
You can explore next samples:
* [Insight.TelegramBot.Samples.SimpleHostedBot](https://github.com/InsightAppDev/Insight.TelegramBot/tree/master/samples/Insight.TelegramBot.Samples.SimpleHostedBot) - Simple polling bot
* [Insight.TelegramBot.Samples.WebHookBot](https://github.com/InsightAppDev/Insight.TelegramBot/tree/master/samples/Insight.TelegramBot.Samples.WebHookBot) - Simple webhook bot
