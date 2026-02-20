using MediatR;

public class StockAvailabilityHandler : INotificationHandler<StockAvailabilityChangedCommand>
{
    private readonly IAdminProductService _productService;
    public StockAvailabilityHandler(IAdminProductService productService)
    {
        _productService = productService;
    }
    public async Task Handle(StockAvailabilityChangedCommand notification, CancellationToken cancellationToken)
    {
        await _productService.ChangeProductAvailabilityAsync(notification.productId);
    }
}