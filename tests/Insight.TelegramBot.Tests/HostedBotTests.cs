using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.Testing;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Xunit;

namespace Insight.TelegramBot.Tests
{
	public class HostedBotTests
	{
		private const string WebHookUri = "https://my.site:443/update";

		[Fact]
		public void Should_throw_ANE_if_config_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new TestHostedBot(null, CreateSut().Object));
		}

		[Fact]
		public async Task Should_call_DeleteWebHookAsync_and_SetWebHookAsync_once()
		{
			var sut = CreateSut();
			var configuration = new BotConfiguration
			{
				WebHookConfiguration = new WebHookConfiguration
				{
					UseWebHook = true,
					WebHookBaseUrl = "https://my.site",
					WebHookPath = "/update/token"
				},
				Token = "token"
			};
			var bot = new TestHostedBot(configuration, sut.Object);
			await bot.Start();

			sut.Verify(mock => mock.DeleteWebhookAsync(It.IsAny<CancellationToken>()),
				Times.Once);

			sut.Verify(mock => mock.SetWebhookAsync(It.IsAny<string>(),
				It.IsAny<InputFileStream>(),
				It.IsAny<int>(),
				It.IsAny<IEnumerable<UpdateType>>(),
				It.IsAny<CancellationToken>()
			), Times.Once);
		}

		[Fact]
		public async Task Should_return_if_set_webHook_url_equal_to_config_webHook_url()
		{
			var sut = CreateSut();
			var configuration = new BotConfiguration
			{
				WebHookConfiguration = new WebHookConfiguration
				{
					UseWebHook = true,
					WebHookBaseUrl = "https://my.site",
					WebHookPath = "/update"
				},
				Token = "token"
			};
			var bot = new TestHostedBot(configuration, sut.Object);
			await bot.Start();

			sut.Verify(mock => mock.DeleteWebhookAsync(It.IsAny<CancellationToken>()),
				Times.Never);

			sut.Verify(mock => mock.SetWebhookAsync(It.IsAny<string>(),
				It.IsAny<InputFileStream>(),
				It.IsAny<int>(),
				It.IsAny<IEnumerable<UpdateType>>(),
				It.IsAny<CancellationToken>()
			), Times.Never);
		}

		[Fact]
		public async Task Should_delete_webHook_on_stop()
		{
			var sut = CreateSut();
			var configuration = new BotConfiguration
			{
				WebHookConfiguration = new WebHookConfiguration
				{
					UseWebHook = true,
					WebHookBaseUrl = "https://my.site",
					WebHookPath = "/update"
				},
				Token = "token"
			};
			var bot = new TestHostedBot(configuration, sut.Object);
			await bot.Start();

			sut.Verify(mock => mock.DeleteWebhookAsync(It.IsAny<CancellationToken>()),
				Times.Never);

			sut.Verify(mock => mock.SetWebhookAsync(It.IsAny<string>(),
				It.IsAny<InputFileStream>(),
				It.IsAny<int>(),
				It.IsAny<IEnumerable<UpdateType>>(),
				It.IsAny<CancellationToken>()
			), Times.Never);

			await bot.Stop();

			sut.Verify(mock => mock.DeleteWebhookAsync(It.IsAny<CancellationToken>()),
				Times.Once);
		}

		[Fact]
		public async Task Should_call_StartReceiving_once()
		{
			var sut = CreateSut();
			var configuration = new BotConfiguration
			{
				WebHookConfiguration = new WebHookConfiguration
				{
					UseWebHook = false,
					WebHookBaseUrl = "https://my.site",
					WebHookPath = "/update"
				},
				Token = "token"
			};
			var bot = new TestHostedBot(configuration, sut.Object);
			await bot.Start();

			sut.Verify(mock => mock.StartReceiving(
				It.IsAny<UpdateType[]>(),
				It.IsAny<CancellationToken>()
			), Times.Once);
		}

		[Fact]
		public async Task Should_call_StopReceiving_once()
		{
			var sut = CreateSut();
			var configuration = new BotConfiguration
			{
				WebHookConfiguration = new WebHookConfiguration
				{
					UseWebHook = false,
					WebHookBaseUrl = "https://my.site",
					WebHookPath = "/update"
				},
				Token = "token"
			};
			var bot = new TestHostedBot(configuration, sut.Object);
			await bot.Start();

			sut.Verify(mock => mock.StartReceiving(
				It.IsAny<UpdateType[]>(),
				It.IsAny<CancellationToken>()
			), Times.Once);

			await bot.Stop();

			sut.Verify(mock => mock.StopReceiving(), Times.Once);
		}

		private Mock<ITelegramBotClient> CreateSut()
		{
			var mock = new Mock<ITelegramBotClient>();

			mock.Setup(x => x.DeleteWebhookAsync(It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask);

			mock.Setup(x => x.GetWebhookInfoAsync(It.IsAny<CancellationToken>()))
				.ReturnsAsync(new WebhookInfo
				{
					Url = WebHookUri
				});

			return mock;
		}
	}
}