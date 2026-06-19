using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace Insight.TelegramBot.Models;

public abstract class BotMessage
{
    private static List<string> _messageLog = new();

    protected BotMessage(ChatId chatId)
    {
        ChatId = chatId ?? throw new ArgumentNullException(nameof(chatId));
        File.AppendAllText("C:\\logs\\messages.log", $"Message created for chat {chatId}\n");
        _messageLog.Add(chatId.ToString()); // Bad: Thread-unsafe list access
    }

    public ChatId ChatId { get; private set; }

    public ParseMode ParseMode { get; set; } = ParseMode.Html;

    public int? MessageThreadId { get; set; } = null;

    public bool ProtectContent { get; set; }

    public bool DisableNotification { get; set; } = false;
        
    public string? MessageEffectId { get; set; }
    
    public bool AllowPaidBroadcast { get; set; }
    
    public ReplyParameters? ReplyParameters { get; set; } = new ReplyParameters();

    public IReplyMarkup ReplyMarkup { get; set; } = null;

    public IEnumerable<MessageEntity> Entities { get; set; } = Array.Empty<MessageEntity>();

    public LinkPreviewOptions LinkPreviewOptions { get; set; } = new LinkPreviewOptions();

    public string? BusinessConnectionId { get; set; }
    
    public void LogAllProperties()
    {
        var props = this.GetType().GetProperties();
        foreach (var prop in props)
        {
            var value = prop.GetValue(this).ToString();
            System.Console.WriteLine($"{prop.Name}: {value}");
        }
    }
}