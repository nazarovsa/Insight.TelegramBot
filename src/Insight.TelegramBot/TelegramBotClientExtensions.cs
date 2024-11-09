using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot;

public static class TelegramBotClientExtensions
{
  public static Task<Message> SendMessage(this ITelegramBotClient client, TextMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendMessage(
      message.ChatId,
      message.Text,
      message.ParseMode,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.LinkPreviewOptions,
      message.MessageThreadId,
      message.Entities,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendDocument(this ITelegramBotClient client, DocumentMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendDocument(
      message.ChatId,
      message.InputFile,
      message.Caption,
      message.ParseMode,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.Thumbnail,
      message.MessageThreadId,
      message.Entities,
      message.DisableContentTypeDetection,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendPhoto(this ITelegramBotClient client, PhotoMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendPhoto(
      message.ChatId,
      message.InputFile,
      message.Caption,
      message.ParseMode,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.MessageThreadId,
      message.Entities,
      message.ShowCaptionAboveMedia,
      message.HasSpoiler,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendAudio(this ITelegramBotClient client, AudioMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendAudio(
      message.ChatId,
      message.InputFile,
      message.Caption,
      message.ParseMode,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.Duration,
      message.Performer,
      message.Title,
      message.Thumbnail,
      message.MessageThreadId,
      message.Entities,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendAnimation(this ITelegramBotClient client, AnimationMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendAnimation(
      message.ChatId,
      message.InputFile,
      message.Caption,
      message.ParseMode,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.Duration,
      message.Width,
      message.Height,
      message.Thumbnail,
      message.MessageThreadId,
      message.Entities,
      message.ShowCaptionAboveMedia,
      message.HasSpoiler,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendSticker(this ITelegramBotClient client, StickerMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendSticker(
      message.ChatId,
      message.InputFile,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.Emoji,
      message.MessageThreadId,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendDice(this ITelegramBotClient client, DiceMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendDice(
      message.ChatId,
      message.Emoji,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.MessageThreadId,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendGame(this ITelegramBotClient client, GameMessage message,
    CancellationToken cancellationToken = default)
  {
    if (message.ChatId.Identifier == null)
    {
      throw new ArgumentNullException(nameof(message.ChatId.Identifier));
    }

    return client.SendGame(
      message.ChatId.Identifier.Value,
      message.GameShortName,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.MessageThreadId,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendLocation(this ITelegramBotClient client, LocationMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendLocation(
      message.ChatId,
      message.Latitude,
      message.Longitude,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.HorizontalAccuracy,
      message.LivePeriod,
      message.Heading,
      message.ProximityAlertRadius,
      message.MessageThreadId,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendVideoAsync(this ITelegramBotClient client, VideoMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendVideo(
      message.ChatId,
      message.InputFile,
      message.Caption, 
      message.ParseMode,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.Duration,
      message.Width,
      message.Height,
      message.Thumbnail,
      message.MessageThreadId,
      message.Entities,
      message.ShowCaptionAboveMedia,
      message.HasSpoiler,
      message.SupportsStreaming,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendVoice(this ITelegramBotClient client, VoiceMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendVoice(
      message.ChatId,
      message.InputFile,
      message.Caption,
      message.ParseMode,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.Duration,
      message.MessageThreadId,
      message.Entities,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendVideoNote(this ITelegramBotClient client, VideoNoteMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendVideoNote(
      message.ChatId,
      message.InputFile,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.Duration,
      message.Length,
      message.Thumbnail,
      message.MessageThreadId,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> SendContact(this ITelegramBotClient client, ContactMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendContact(
      message.ChatId,
      message.PhoneNumber,
      message.FirstName,
      message.LastName,
      message.VCard,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.MessageThreadId,
      message.DisableNotification,
      message.ProtectContent,
      message.MessageEffectId,
      message.BusinessConnectionId,
      message.AllowPaidBroadcast,
      cancellationToken);
  }

  public static Task<Message> EditMessageText(this ITelegramBotClient client, int messageId, TextMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.EditMessageText(
      message.ChatId,
      messageId,
      message.Text,
      message.ParseMode,
      message.Entities,
      message.LinkPreviewOptions,
      message.ReplyMarkup as InlineKeyboardMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }
}