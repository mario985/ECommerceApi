
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

[Route("Api/[controller]")]
[ApiController]
public class AdminProductController : ControllerBase
{
    private readonly IProductService _productService;
    public AdminProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto createProductDto)
    {
        await _productService.AddProductAsync(createProductDto);
        return NoContent();
    }
     [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto updateProductDto)
    {
        var result = await _productService.UpdateProductAsync(id, updateProductDto);
        if (result.Success == true)
        {
            return NoContent();
        }
        else return BadRequest(new List<string> { result.Message });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (result.Success == true)
        {
            return NoContent();
        }
        else return BadRequest(new List<string> { result.Message });
    }
   
    
}