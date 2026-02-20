public interface IStripePaymentService
{
    Task<(string paymentIntentId, string clientSecret)> CreatePaymentIntentForOrderAsync(Order order);
}
