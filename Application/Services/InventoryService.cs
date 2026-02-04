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
            existing.OnHand += quantity;
            await _inventoryRepository.UpdateAsync(existing);
        }
        else await CreateInventoryAsync(productId , quantity);
    }

    public async Task ChangeStockAsync(string productId, int quantity)
    {
        ValidateQuantity(quantity);

        var existing = await _inventoryRepository.GetByProductIdAsync(productId);
        if (existing is not null)
        {
            existing.OnHand = quantity;
            await _inventoryRepository.UpdateAsync(existing);
        }
        else await CreateInventoryAsync(productId , quantity);
    }

    public async Task<Inventory> GetInventoryAsync(string productId)
    {
        var inventory =  await _inventoryRepository.GetByProductIdAsync(productId);
        return (inventory==null) ?await CreateInventoryAsync(productId , 0) : inventory;

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
    private async Task <Inventory> CreateInventoryAsync(string productId , int quantity)
    {
        var newInventory = new Inventory
            {
                ProductId = productId,
                OnHand= quantity,
                Reserved = 0
            };
            await _inventoryRepository.AddAsync(newInventory);
            return  newInventory;
            
    }

    public async Task ReserveStockAsync(string productId, int reduceBy)
    {
        ValidateQuantity(reduceBy);
        await GetInventoryAsync(productId);
       var success =  await _inventoryRepository.TryApplyStockTransitionAsync(productId , reduceBy , StockTransition.Reserve);
        if (!success)
        {
           throw new InvalidOperationException("insufficent stock");
        }

    }

    public async Task CommitReservedStockAsync(string productId, int reduceBy)
    {
          ValidateQuantity(reduceBy);
        await GetInventoryAsync(productId);
       var success =  await _inventoryRepository.TryApplyStockTransitionAsync(productId , reduceBy , StockTransition.Commit);
        if (!success)
        {
           throw new InvalidOperationException("insufficent stock");
        }
    }

    public async Task ReleaseReservedStockAsync(string productId, int reduceBy)
    {
          ValidateQuantity(reduceBy);
        await GetInventoryAsync(productId);
       var success =  await _inventoryRepository.TryApplyStockTransitionAsync(productId , reduceBy , StockTransition.Release);
        if (!success)
        {
           throw new InvalidOperationException("insufficent stock");
        }
    }
}
