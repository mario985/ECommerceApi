using MediatR;

public class ProductUpdatedHandler : INotificationHandler<ProductUpdatedCommand>
{
    private readonly IInvetoryService _invetoryService;
    public ProductUpdatedHandler(IInvetoryService invetoryService)
    {
        _invetoryService = invetoryService;
    }
    public async Task Handle(ProductUpdatedCommand notification, CancellationToken cancellationToken)
    {
        await _invetoryService.ChangeStockAsync(notification.productId, notification.quantity);
    }
}