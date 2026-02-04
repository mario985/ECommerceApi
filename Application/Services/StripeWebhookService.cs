using Stripe;

public class StripeWebhookService : IStripeWebhookService
{
    private readonly IConfiguration _config;
    private readonly IorderRepository _orderRepository;
    private readonly IInventoryService _inventoryService;
    private readonly IUnitOfWork _unitOfWork;

    public StripeWebhookService(
        IConfiguration config,
        IorderRepository orderRepository,
        IInventoryService inventoryService,
        IUnitOfWork unitOfWork)
    {
        _config = config;
        _orderRepository = orderRepository;
        _inventoryService = inventoryService;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(string json, string stripeSignatureHeader)
    {
        var webhookSecret = _config["Stripe:WebhookSecret"];
        if (string.IsNullOrWhiteSpace(webhookSecret))
            throw new InvalidOperationException("Stripe webhook secret is not configured.");

        Event stripeEvent = ConstructAndVerifyEvent(json, stripeSignatureHeader, webhookSecret);

       switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                await HandlePaymentIntentSucceeded(stripeEvent);
                break;

            case "payment_intent.payment_failed":
                await HandlePaymentIntentFailed(stripeEvent);
                break;

            default:
                // Ignore unhandled events
                break;
        }

    }

    private static Event ConstructAndVerifyEvent(string json, string signatureHeader, string webhookSecret)
    {
        try
        {
            return EventUtility.ConstructEvent(json, signatureHeader, webhookSecret);
        }
        catch (StripeException ex)
        {
            throw new StripeSignatureException("Invalid Stripe webhook signature.", ex);
        }
    }

    private async Task HandlePaymentIntentSucceeded(Event stripeEvent)
    {
        var intent = stripeEvent.Data.Object as PaymentIntent;
        if (intent == null) return;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await _orderRepository.GetbyPaymentIntentIdAsync(intent.Id);
            if (order == null)
            {
                await _unitOfWork.CommitAsync();
                return;
            }

            if (order.Status == OrderStatus.Paid)
            {
                await _unitOfWork.CommitAsync();
                return;
            }

            foreach (var item in order.OrderItems)
                await _inventoryService.CommitReservedStockAsync(item.ProductId, item.Quantity);

            order.Status = OrderStatus.Paid;
            order.PaymentConfirmedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task HandlePaymentIntentFailed(Event stripeEvent)
    {
        var intent = stripeEvent.Data.Object as PaymentIntent;
        if (intent == null) return;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await _orderRepository.GetbyPaymentIntentIdAsync(intent.Id);
            if (order == null)
            {
                await _unitOfWork.CommitAsync();
                return;
            }

            if (order.Status == OrderStatus.Paid || order.Status == OrderStatus.PaymentFailed)
            {
                await _unitOfWork.CommitAsync();
                return;
            }

            foreach (var item in order.OrderItems)
                await _inventoryService.ReleaseReservedStockAsync(item.ProductId, item.Quantity);

            order.Status = OrderStatus.PaymentFailed;

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}

public class StripeSignatureException : Exception
{
    public StripeSignatureException(string message, Exception? inner = null) : base(message, inner) { }
}
