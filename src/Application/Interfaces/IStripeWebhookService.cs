public interface IStripeWebhookService
{
    Task HandleAsync(string json, string stripeSignatureHeader);
}
