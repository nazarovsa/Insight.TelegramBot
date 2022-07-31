using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Models
{
    public abstract class BotMessage
    {
        protected BotMessage(ChatId chatId)
        {
            ChatId = chatId ?? throw new ArgumentNullException(nameof(chatId));
        }

        public ChatId ChatId { get; private set; }

        public ParseMode ParseMode { get; set; } = ParseMode.Html;
        
        public bool ProtectContent { get; set; }

        public bool DisableNotification { get; set; } = false;

        public int ReplyToMessageId { get; set; } = 0;

        public IReplyMarkup ReplyMarkup { get; set; } = null;
        
        public IEnumerable<MessageEntity> Entities { get; set; } = Array.Empty<MessageEntity>();
        
        public bool AllowSendingWithoutReply { get; set; }
    }
}