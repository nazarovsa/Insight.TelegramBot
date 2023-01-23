using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Insight.TelegramBot.UpdateProcessors;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Samples.Domain
{
    public sealed class SampleUpdateProcessor : IPollingUpdateProcessor
    {
        private readonly IBot _bot;

        public SampleUpdateProcessor(IBot bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public Task HandleUpdate(Update update, CancellationToken cancellationToken = default)
        {
            return update.Type switch
            {
                UpdateType.Message => ProcessMessage(update.Message, cancellationToken),
                UpdateType.CallbackQuery => ProcessCallback(update.CallbackQuery, cancellationToken),
                _ => throw new NotImplementedException()
            };
        }

        public Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessMessage(Message message, CancellationToken cancellationToken = default)
        {
            if (message.Text != null)
            {
                if (message.Text == "/start")
                {
                    await _bot.SendMessageAsync(new TextMessage(message.Chat.Id)
                    {
                        Text = "Hello world",
                        ReplyMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
                        {
                            new List<InlineKeyboardButton>
                            {
                                new InlineKeyboardButton("Touch me.")
                                {
                                    CallbackData = new CallbackData<SampleState>(SampleState.TouchMe).ToString()
                                }
                            }
                        })
                    }, cancellationToken);
                }
            }
        }

        private async Task ProcessCallback(CallbackQuery callbackQuery, CancellationToken cancellationToken = default)
        {
            var callbackData = CallbackData<SampleState>.Parse(callbackQuery.Data);

            switch (callbackData.NextState)
            {
                case SampleState.TouchMe:
                    await _bot.SendMessageAsync(new TextMessage(callbackQuery.From.Id)
                    {
                        Text = "Oh my",
                        ReplyMarkup = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
                        {
                            new List<InlineKeyboardButton>
                            {
                                new InlineKeyboardButton("Touch me")
                                {
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
    }
}