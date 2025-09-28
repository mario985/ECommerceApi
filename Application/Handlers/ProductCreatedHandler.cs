using MediatR;

public class ProductCreatedHandler : INotificationHandler<ProductCreated>
{
    private readonly IInvetoryService _invetoryService;
    public ProductCreatedHandler(IInvetoryService invetoryService)
    {
        _invetoryService = invetoryService;
    }
    public async Task Handle(ProductCreated notification, CancellationToken cancellationToken)
    {
        await _invetoryService.AddStockAsync(notification.ProductId, notification.quantity);
    }
}