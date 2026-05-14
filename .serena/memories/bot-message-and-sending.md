# Bot Messages & Sending

## When to consult
When adding a new outbound message type, a new `ITelegramBotClient` send extension, or modifying parameter forwarding.

## Rules
- Every send-side message is a POCO under `src/Insight.TelegramBot/Models/`, deriving from `BotMessage` (text/no-file) or `BotMessageWithFile` (anything with an `InputFile`).
  - `BotMessage` (`Models/Base/BotMessage.cs`) — ctor `protected BotMessage(ChatId chatId)` null-checks via `?? throw new ArgumentNullException(nameof(chatId))`. Holds shared fields: `ChatId`, `ParseMode = ParseMode.Html` (default), `MessageThreadId`, `ProtectContent`, `DisableNotification`, `MessageEffectId`, `AllowPaidBroadcast`, `ReplyParameters = new ReplyParameters()`, `ReplyMarkup`, `Entities = Array.Empty<MessageEntity>()`, `LinkPreviewOptions = new LinkPreviewOptions()`, `BusinessConnectionId`.
  - `BotMessageWithFile` adds `InputFile InputFile { get; }` (set in ctor), `Thumbnail`, `HasSpoiler`, `Caption`, `ShowCaptionAboveMedia`.
- A new message type follows the shape of the closest existing peer: e.g. `DocumentMessage : BotMessageWithFile` adds only `DisableContentTypeDetection`; `TextMessage : BotMessage` adds only `Text`. Use auto-properties; no comments.
- Sending goes through static extensions in `src/Insight.TelegramBot/TelegramBotClientExtensions.cs`. Each method:
  - Is named `Send<MessageKind>` (or `EditMessage<Kind>`) and takes `(this ITelegramBotClient client, TXxxMessage message, CancellationToken cancellationToken = default)`.
  - Returns `Task<Message>` and just forwards every property to the corresponding `Telegram.Bot` extension. The order of arguments matches the SDK signature — keep them in order so additions don't silently shift bindings.
  - The send methods are thin pass-throughs — no validation, no logging. Validation belongs in the message POCO (see `WebHookOptions.WebHookUrl` for the lazy-throw pattern on derived data) or in the caller.
- `SendGame` is the one exception: it explicitly null-checks `message.ChatId.Identifier` and throws `ArgumentNullException` (because `Telegram.Bot.SendGame` requires a `long`, not a `ChatId`). Follow the same pattern when an underlying API needs a narrower type than the message POCO holds.
- One naming inconsistency: `SendVideoAsync` is the only one with the `Async` suffix (`TelegramBotClientExtensions.cs:206`). All other extensions drop the suffix. Default to no suffix; only keep `SendVideoAsync` for back-compat — if you rename it, treat it as a breaking change.

## Canonical examples in this codebase
- `src/Insight.TelegramBot/Models/Base/BotMessage.cs` + `BotMessageWithFile.cs` — base classes and default values.
- `src/Insight.TelegramBot/Models/TextMessage.cs` — minimal `BotMessage` derivative.
- `src/Insight.TelegramBot/Models/DocumentMessage.cs` — minimal `BotMessageWithFile` derivative.
- `src/Insight.TelegramBot/TelegramBotClientExtensions.cs:13-31` (`SendMessage`) — canonical pass-through extension shape.

## Anti-patterns
- Building a `ChatId` inline inside the extension method. Callers construct messages with a `ChatId`; the extension just hands it off.
- Defaulting properties to `null` in the message POCO when a sensible default object exists (compare `ReplyParameters = new ReplyParameters()` vs leaving it `null`).
- Adding XML doc comments to send extensions — none of the existing extensions carry them. Stay consistent or add docs everywhere in one pass.
- Mixing send + receive concerns. Send extensions live in `Insight.TelegramBot`; nothing about `IUpdateProcessor`/handlers belongs there.

See also: [[architecture-overview]].
