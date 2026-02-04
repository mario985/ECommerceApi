using Stripe;

public class StripePaymentService : IStripePaymentService
{
    private readonly PaymentIntentService _paymentIntentService;

    public StripePaymentService(PaymentIntentService paymentIntentService)
    {
        _paymentIntentService = paymentIntentService;
    }

    public async Task<(string paymentIntentId, string clientSecret)> CreatePaymentIntentForOrderAsync(Order order)
    {
        // Stripe expects "smallest currency unit" (cents/piastres)
        // If TotalPrice is decimal in "EGP pounds", convert properly.
        long amount = ConvertToSmallestUnit(order.TotalPrice, order.Currency);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = order.Currency.ToLower(), // "egp", "usd"
            ReturnUrl = "never",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            },
            Metadata = new Dictionary<string, string>
            {
                { "orderId", order.Id.ToString() },
                { "userId", order.UserId }
            }
        };

        // Idempotency: if client retries the create call, you don’t create multiple PaymentIntents
        var requestOptions = new RequestOptions
        {
            IdempotencyKey = $"pi_order_{order.Id}"
        };

        var intent = await _paymentIntentService.CreateAsync(options, requestOptions);
        return (intent.Id, intent.ClientSecret);
    }

    private static long ConvertToSmallestUnit(decimal totalPrice, string currency)
    {
        // Most currencies have 2 decimals. Some have 0.
        // For a practice project: assume 2 decimals for egp/usd
        return (long)Math.Round(totalPrice * 100m, MidpointRounding.AwayFromZero);
    }
}
