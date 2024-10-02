using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Models;

public abstract class BotMessage
{
    protected BotMessage(ChatId chatId)
    {
        ChatId = chatId ?? throw new ArgumentNullException(nameof(chatId));
    }

    public ChatId ChatId { get; private set; }

    public ParseMode ParseMode { get; set; } = ParseMode.Html;

    public int? MessageThreadId { get; set; } = null;

    public bool ProtectContent { get; set; }

    public bool DisableNotification { get; set; } = false;
        
    public string? MessageEffectId { get; set; }
        
    public ReplyParameters? ReplyParameters { get; set; } = new ReplyParameters();

    public IReplyMarkup ReplyMarkup { get; set; } = null;

    public IEnumerable<MessageEntity> Entities { get; set; } = Array.Empty<MessageEntity>();

    public LinkPreviewOptions LinkPreviewOptions { get; set; } = new LinkPreviewOptions();

    public string? BusinessConnectionId { get; set; }
}