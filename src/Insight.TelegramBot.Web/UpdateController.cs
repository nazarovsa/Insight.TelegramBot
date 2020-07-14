using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Insight.TelegramBot.Web
{
    public sealed class UpdateController : ControllerBase
    {
        private readonly ILogger<UpdateController> _logger;

        private readonly IBotProcessor _processor;

        public UpdateController(ILogger<UpdateController> logger, IBotProcessor processor)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (processor == null)
                throw new ArgumentNullException(nameof(processor));

            _logger = logger;
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            if (update == null)
                return BadRequest();

            if (update.Type == UpdateType.Message)
            { 
                _logger.LogTrace($"Received message from: {update.Message.From.Id}");
                await _processor.ProcessMessage(update.Message, cancellationToken);
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                _logger.LogTrace($"Received callback query from: {update.CallbackQuery.From.Id}");
                await _processor.ProcessCallback(update.CallbackQuery, cancellationToken);
            }
            else
            {
                throw new NotImplementedException();
            }

            return Ok();
        }
    }
}