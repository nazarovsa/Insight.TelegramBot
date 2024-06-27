using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.UpdateProcessors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.WebHook.Controllers;

[Controller]
public sealed class DefaultUpdateController : ControllerBase
{
    private readonly ILogger<DefaultUpdateController> _logger;

    private readonly IUpdateProcessor _processor;

    public DefaultUpdateController(ILogger<DefaultUpdateController> logger, IUpdateProcessor processor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        if (update == null)
        {
            _logger.LogWarning("Received null update");
            return BadRequest();
        }

        try
        {
            _logger.LogDebug("Received update: {@Update}", update);
            _logger.LogTrace("Received update id: {UpdateId}", update.Id);

            await _processor.HandleUpdate(update, cancellationToken);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at processing update: {UpdateId}", update.Id);
            return BadRequest();
        }
    }
}