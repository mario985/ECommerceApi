using Stripe;

public class OrderExpirationService : IOrderExpirationService
{
    private readonly IorderRepository _orderRepository;
    private readonly IInventoryService _inventoryService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PaymentIntentService _paymentIntentService;

    public OrderExpirationService(
        IorderRepository orderRepository,
        IInventoryService inventoryService,
        IUnitOfWork unitOfWork,
        PaymentIntentService paymentIntentService)
    {
        _orderRepository = orderRepository;
        _inventoryService = inventoryService;
        _unitOfWork = unitOfWork;
        _paymentIntentService = paymentIntentService;
    }

    public async Task ExpirePendingOrdersAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var expired = await _orderRepository.GetExpiredPendingAsync(now, take: 50);

        foreach (var order in expired)
        {
            ct.ThrowIfCancellationRequested();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Re-load / re-check status (idempotency / race safety)
                var fresh = await _orderRepository.GetAsync(order.Id);
                if (fresh == null || fresh.Status != OrderStatus.PendingPayment)
                {
                    await _unitOfWork.CommitAsync();
                    continue;
                }

                // Release reserved stock
                foreach (var item in order.OrderItems!)
                    await _inventoryService.ReleaseReservedStockAsync(item.ProductId, item.Quantity);

                // Optional: cancel PaymentIntent at Stripe to stop late payments
                if (!string.IsNullOrWhiteSpace(order.StripePaymentIntentId))
                {
                    try { await _paymentIntentService.CancelAsync(order.StripePaymentIntentId); }
                    catch { /* ignore if already paid/canceled */ }
                }

                // Mark order expired
                order.Status = OrderStatus.Cancelled; // or Cancelled
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
}
