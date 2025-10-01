using MediatR;

public class InventoryService :IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IMediator _mediator;

    public InventoryService(IInventoryRepository inventoryRepository, IMediator mediator)
    {
        _inventoryRepository = inventoryRepository;
        _mediator = mediator;
    }

    public async Task AddStockAsync(string productId, int quantity)
    {
        ValidateQuantity(quantity);

        var existing = await _inventoryRepository.GetByProductIdAsync(productId);
        if (existing is not null)
        {
            existing.Quantity += quantity;
            await _inventoryRepository.UpdateAsync(existing);
        }
        else
        {
            var newInventory = new Inventory
            {
                ProductId = productId,
                Quantity = quantity
            };
            await _inventoryRepository.AddAsync(newInventory);
        }
    }

    public async Task ChangeStockAsync(string productId, int newQuantity)
    {
        ValidateQuantity(newQuantity);

        var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
        if (inventory is null)
        {
            throw new KeyNotFoundException("Inventory not found");
        }

        inventory.Quantity = newQuantity;
        await _inventoryRepository.UpdateAsync(inventory);
    }

    public async Task<Inventory?> GetInventoryAsync(string productId)
    {
        return await _inventoryRepository.GetByProductIdAsync(productId);
    }

    public async Task ReduceQuantityAsync(string productId, int reduceBy)
    {
        ValidateQuantity(reduceBy);

        var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
        if (inventory is null)
        {
            throw new KeyNotFoundException("Inventory not found");
        }

        if (inventory.Quantity < reduceBy)
        {
            throw new InvalidOperationException("Insufficient stock to reduce");
        }

        inventory.Quantity -= reduceBy;
        await _inventoryRepository.UpdateAsync(inventory);

        if (inventory.Quantity <= 0)
        {
            await _mediator.Publish(new StockAvailabilityChangedCommand(productId));
        }
    }

    public async Task RemoveAsync(string productId)
    {
        var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
        if (inventory is null)
        {
            throw new KeyNotFoundException("Inventory not found");
        }

        await _inventoryRepository.RemoveAsync(inventory);
    }

    private void ValidateQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("Quantity can't be negative", nameof(quantity));
        }
    }
}
