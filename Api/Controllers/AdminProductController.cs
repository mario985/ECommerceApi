
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

[Route("Api/[controller]")]
[ApiController]
public class AdminProductController : ControllerBase
{
    private readonly IAdminProductService _adminProductService;
    public AdminProductController(IAdminProductService adminProductService)
    {
        _adminProductService = adminProductService;
    }
    [HttpPost]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Create(CreateProductDto createProductDto)
    {
        await _adminProductService.AddProductAsync(createProductDto);
        return NoContent();
    }
    [HttpPut("{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto updateProductDto)
    {
        var result = await _adminProductService.UpdateProductAsync(id, updateProductDto);
        if (result.Success == true)
        {
            return NoContent();
        }
        else return BadRequest(new List<string> { result.Message });
    }
    [HttpDelete("{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _adminProductService.DeleteProductAsync(id);
        if (result.Success == true)
        {
            return NoContent();
        }
        else return BadRequest(new List<string> { result.Message });
    }
   
    
}