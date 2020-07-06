using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Configurations;
using Insight.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Samples.Domain
{
    public sealed class SampleHostedBot : HostedBot
    {
        public SampleHostedBot(BotConfiguration config, ITelegramBotClient client) : base(
            config, client)
        {
        }

        public override async Task ProcessMessage(Message message, CancellationToken token = default)
        {
            if (message.Text != null)
            {
                if (message.Text == "/start")
                {
                    await SendMessage(new BotMessage
                    {
                        ChatId = message.Chat.Id,
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
                    }, token);
                }
            }
        }

        public override async Task ProcessCallback(CallbackQuery query, CancellationToken token = default)
        {
            var callbackData = CallbackData<SampleState>.Parse(query.Data);

            switch (callbackData.NextState)
            {
                case SampleState.TouchMe:
                    await SendMessage(new BotMessage
                    {
                        ChatId = query.From.Id,
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
                    }, token);
                    break;
                default:
                    throw new ArgumentException(nameof(callbackData.NextState));
            }
        }
    }
}