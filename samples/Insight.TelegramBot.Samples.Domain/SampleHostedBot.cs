using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Samples.Domain
{
    public sealed class SampleHostedBot : HostedBot
    {
        public SampleHostedBot(BotConfiguration config, ITelegramBotClient client)
            : base(config, client)
        {
        }

        public override Task ProcessInlineQuery(InlineQuery inlineQuery, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ProcessUpdate(Update message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override async Task ProcessMessage(Message message, CancellationToken cancellationToken = default)
        {
            if (message.Text != null)
            {
                if (message.Text == "/start")
                {
                    await SendMessageAsync(new TextMessage(message.Chat.Id)
                    {
                        Text = "Hello world",
                        ReplyMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
                        {
                            new List<InlineKeyboardButton>
                            {
                                new InlineKeyboardButton
                                {
                                    Text = "Touch me",
                                    CallbackData = new CallbackData<SampleState>(SampleState.TouchMe).ToString()
                                }
                            }
                        })
                    }, cancellationToken);
                }
            }
        }

        public override async Task ProcessCallback(CallbackQuery callbackQuery,
            CancellationToken cancellationToken = default)
        {
            var callbackData = CallbackData<SampleState>.Parse(callbackQuery.Data);

            switch (callbackData.NextState)
            {
                case SampleState.TouchMe:
                    await SendMessageAsync(new TextMessage(callbackQuery.From.Id)
                    {
                        Text = "Oh my",
                        ReplyMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
                        {
                            new List<InlineKeyboardButton>
                            {
                                new InlineKeyboardButton
                                {
                                    Text = "Touch me",
                                    CallbackData = new CallbackData<SampleState>(SampleState.TouchMe).ToString()
                                }
                            }
                        })
                    }, cancellationToken);
                    break;
                default:
                    throw new ArgumentException(nameof(callbackData.NextState));
            }
        }

        public override Task ProcessReceiveError(ApiRequestException exception,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}