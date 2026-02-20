using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("Api/Admin/inventory")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    public InventoryController( IInventoryService inventoryService)
    {
        _inventoryService  = inventoryService;
    }
    [HttpPost("{ProductId}/Adjust")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult>AdjustStock( string ProductId ,[FromBody]AdjustSTockDto adjustSTockDto )
    {
        await _inventoryService.ChangeStockAsync(ProductId,adjustSTockDto.delta);
        return NoContent();
    }
}