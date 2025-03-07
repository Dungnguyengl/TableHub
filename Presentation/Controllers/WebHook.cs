using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace Presentation.Controllers
{
    [Route("/web-hooks")]
    public class WebHook(ILogger<WebHook> logger) : ControllerBase
    {
        private readonly ILogger<WebHook> _logger = logger;

        [HttpPost("payment-info")]
        public IActionResult PaymentInfo([FromBody] WebhookType input)
        {
            _logger.LogInformation("Received input: {@Input}", input);
            return Ok(new { Success = true });
        }
    }
}
