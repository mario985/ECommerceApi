using MediatR;

public class ProductCreatedHandler : INotificationHandler<ProductCreated>
{
    private readonly IInventoryService _invetoryService;
    public ProductCreatedHandler(IInventoryService invetoryService)
    {
        _invetoryService = invetoryService;
    }
    public async Task Handle(ProductCreated notification, CancellationToken cancellationToken)
    {
        await _invetoryService.ChangeStockAsync(notification.ProductId, notification.quantity);
    }
}