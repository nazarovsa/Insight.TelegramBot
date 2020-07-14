using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Testing;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Xunit;

namespace Insight.TelegramBot.Tests
{
    public class HostedBotTests
    {
        private const string WebHookUri = "https://my.site/update";

        [Fact]
        public void Should_throw_ANE_if_config_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new TestHostedBot(null, CreateSut()));
        }

        private ITelegramBotClient CreateSut()
        {
            var mock = new Mock<ITelegramBotClient>();

            mock.Setup(x => x.DeleteWebhookAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mock.Setup(x => x.GetWebhookInfoAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WebhookInfo
                {
                    Url = WebHookUri
                });

            return mock.Object;
        }
    }
}