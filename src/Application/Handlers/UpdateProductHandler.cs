using MediatR;

public class ProductUpdatedHandler : INotificationHandler<ProductUpdatedCommand>
{
    private readonly IInventoryService _invetoryService;
    public ProductUpdatedHandler(IInventoryService invetoryService)
    {
        _invetoryService = invetoryService;
    }
    public async Task Handle(ProductUpdatedCommand notification, CancellationToken cancellationToken)
    {
        await _invetoryService.ChangeStockAsync(notification.productId, notification.quantity);
    }
}