using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Moq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Insight.TelegramBot.Tests
{
    public sealed class BotTests
    {
        private ITelegramBotClient _client;

        public BotTests()
        {
            _client = CreateSut();
        }

        [Fact]
        public void Should_throw_ANE_if_client_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new Bot(null));
        }

        [Fact]
        public async Task Should_send_message()
        {
            var bot = new Bot(_client);
            var chatId = 1;
            var text = "Hello world";
            var message = await bot.SendMessageAsync(new TextMessage(chatId)
            {
                Text = text
            });
            Assert.NotNull(message);
            Assert.Equal(message.Chat.Id, chatId);
            Assert.Equal(message.Text, text);
        }

        [Fact]
        public async Task Should_delete_message()
        {
            var bot = new Bot(_client);
            await bot.DeleteMessageAsync(0, 0);
        }

        private ITelegramBotClient CreateSut()
        {
            var mock = new Mock<ITelegramBotClient>();
            mock
                .Setup(x => x.SendTextMessageAsync(It.IsAny<ChatId>(),
                    It.IsAny<string>(),
                    It.IsAny<ParseMode>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<int>(),
                    It.IsAny<IReplyMarkup>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((ChatId chatId,
                        string text,
                        ParseMode parseMode,
                        bool disableWebPagePreview,
                        bool disableNotification,
                        int replyToMessageId,
                        IReplyMarkup replyMarkup,
                        CancellationToken cancellationToken) =>
                    new Message {Chat = new Chat {Id = chatId.Identifier}, Text = text});

            mock.Setup(x => x.DeleteMessageAsync(It.IsAny<ChatId>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            return mock.Object;
        }
    }
}