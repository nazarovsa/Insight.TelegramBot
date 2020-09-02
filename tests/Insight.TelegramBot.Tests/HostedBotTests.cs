using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.Testing;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
		public void Should_call_StartReceiving_once()
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
			bot.Start();

			sut.Verify(mock => mock.StartReceiving(
				It.IsAny<UpdateType[]>(),
				It.IsAny<CancellationToken>()
			), Times.Once);
		}

		[Fact]
		public void Should_call_StopReceiving_once()
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
			bot.Start();

			sut.Verify(mock => mock.StartReceiving(
				It.IsAny<UpdateType[]>(),
				It.IsAny<CancellationToken>()
			), Times.Once);

			bot.Stop();

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