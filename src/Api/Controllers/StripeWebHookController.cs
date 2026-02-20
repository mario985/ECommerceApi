using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/webhooks/stripe")]
public class StripeWebhookController : ControllerBase
{
    private readonly IStripeWebhookService _stripeWebhookService;

    public StripeWebhookController(IStripeWebhookService stripeWebhookService)
    {
        _stripeWebhookService = stripeWebhookService;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var stripeSignature = Request.Headers["Stripe-Signature"].ToString();

        try
        {
            await _stripeWebhookService.HandleAsync(json, stripeSignature);
            return Ok();
        }
        catch (StripeSignatureException)
        {
            // Stripe signature invalid => tell Stripe it's bad request
            return BadRequest();
        }
    }
}
