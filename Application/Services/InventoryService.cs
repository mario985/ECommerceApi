
using MediatR;

public class InventoryService : IInvetoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IMediator _mediator;
    public InventoryService(IInventoryRepository inventoryRepository , IMediator mediator)
    {
        _inventoryRepository = inventoryRepository;
        _mediator = mediator;
    }
    public async Task AddStockAsync(string productId, int quantity)
    {
        checkQuantity(quantity);
        await _inventoryRepository.AddStockAsync(productId, quantity);
    }

    public async Task ChangeStockAsync(string productId, int quantity)
    {
        checkQuantity(quantity);
        
        await _inventoryRepository.UpdateStockAsync(productId, quantity);
    }

    public async Task<Inventory> GetInventoryAsync(string ProductId)
    {
        return await _inventoryRepository.GetByProductIdAsync(ProductId);
    }

    public async Task ReduceQuantityAsync(string productId, int quantity)
    {
        checkQuantity(quantity);
        await _inventoryRepository.UpdateStockAsync(productId, quantity);
        if (quantity <= 0)
            await _mediator.Publish(new StockAvailabilityChangedCommand(productId));
       

    }
    private void checkQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new Exception("quantity can't be negative number");
        }
    }
}