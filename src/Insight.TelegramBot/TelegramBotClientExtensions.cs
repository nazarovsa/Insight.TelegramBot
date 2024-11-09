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
  public static Task<Message> SendMessageAsync(this ITelegramBotClient client, TextMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendTextMessageAsync(
      message.ChatId,
      message.Text,
      message.MessageThreadId,
      message.ParseMode,
      message.Entities,
      message.LinkPreviewOptions,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendDocumentAsync(this ITelegramBotClient client, DocumentMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendDocumentAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Thumbnail,
      message.Caption,
      message.ParseMode,
      message.Entities,
      message.DisableContentTypeDetection,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendPhotoAsync(this ITelegramBotClient client, PhotoMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendPhotoAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Caption,
      message.ParseMode,
      message.Entities,
      message.ShowCaptionAboveMedia,
      message.HasSpoiler,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendAudioAsync(this ITelegramBotClient client, AudioMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendAudioAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Caption,
      message.ParseMode,
      message.Entities,
      message.Duration,
      message.Performer,
      message.Title,
      message.Thumbnail,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendAnimationAsync(this ITelegramBotClient client, AnimationMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendAnimationAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Duration,
      message.Width,
      message.Height,
      message.Thumbnail,
      message.Caption,
      message.ParseMode,
      message.Entities,
      message.ShowCaptionAboveMedia,
      message.HasSpoiler,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendStickerAsync(this ITelegramBotClient client, StickerMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendStickerAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Emoji,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendDiceAsync(this ITelegramBotClient client, DiceMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendDiceAsync(
      message.ChatId,
      message.MessageThreadId,
      message.Emoji,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendGameAsync(this ITelegramBotClient client, GameMessage message,
    CancellationToken cancellationToken = default)
  {
    if (message.ChatId.Identifier == null)
    {
      throw new ArgumentNullException(nameof(message.ChatId.Identifier));
    }

    return client.SendGameAsync(
      message.ChatId.Identifier.Value,
      message.GameShortName,
      message.MessageThreadId,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendLocationAsync(this ITelegramBotClient client, LocationMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendLocationAsync(
      message.ChatId,
      message.Latitude,
      message.Longitude,
      message.MessageThreadId,
      message.HorizontalAccuracy,
      message.LivePeriod,
      message.Heading,
      message.ProximityAlertRadius,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendVideoAsync(this ITelegramBotClient client, VideoMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendVideoAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Duration,
      message.Width,
      message.Height,
      message.Thumbnail,
      message.Caption,
      message.ParseMode,
      message.Entities,
      message.ShowCaptionAboveMedia,
      message.HasSpoiler,
      message.SupportsStreaming,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendVoiceAsync(this ITelegramBotClient client, VoiceMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendVoiceAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Caption,
      message.ParseMode,
      message.Entities,
      message.Duration,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendVideoNoteAsync(this ITelegramBotClient client, VideoNoteMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendVideoNoteAsync(
      message.ChatId,
      message.InputFile,
      message.MessageThreadId,
      message.Duration,
      message.Length,
      message.Thumbnail,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> SendContactAsync(this ITelegramBotClient client, ContactMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.SendContactAsync(
      message.ChatId,
      message.PhoneNumber,
      message.FirstName,
      message.MessageThreadId,
      message.LastName,
      message.VCard,
      message.DisableNotification,
      message.ProtectContent,
      message.AllowPaidBroadcast,
      message.MessageEffectId,
      message.ReplyParameters,
      message.ReplyMarkup,
      message.BusinessConnectionId,
      cancellationToken);
  }

  public static Task<Message> EditMessageTextAsync(this ITelegramBotClient client, int messageId, TextMessage message,
    CancellationToken cancellationToken = default)
  {
    return client.EditMessageTextAsync(
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