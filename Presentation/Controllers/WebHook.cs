using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace Presentation.Controllers
{
    [Route("/web-hooks")]
    public class WebHook : ControllerBase
    {
        [HttpGet("payment-info")]
        public WebhookData PaymentInfo(WebhookType input)
        {
            throw new NotImplementedException();
        }
    }
}
