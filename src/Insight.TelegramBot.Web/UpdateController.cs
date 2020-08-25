using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Web
{
    public sealed class UpdateController : ControllerBase
    {
        private readonly ILogger<UpdateController> _logger;

        private readonly IUpdateProcessor _processor;

        public UpdateController(ILogger<UpdateController> logger, IUpdateProcessor processor)
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

            try
            {
                _logger.LogTrace($"Received update from: {update.Message.From.Id}");

                await _processor.ProcessUpdate(update, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error at processing update");
                return BadRequest();
            }
        }
    }
}