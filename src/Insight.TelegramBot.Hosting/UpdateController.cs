using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.TelegramBot.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Insight.TelegramBot.Hosting;

public sealed class UpdateController : ControllerBase
{
    private readonly ILogger<UpdateController> _logger;

    private readonly IUpdateProcessor _processor;

    public UpdateController(ILogger<UpdateController> logger, IUpdateProcessor processor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        if (update == null)
            return BadRequest();

        try
        {
            _logger.LogTrace($"Received update id: {update.Id}");

            await _processor.HandleUpdate(update, cancellationToken);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at processing update");
            return BadRequest();
        }
    }
}