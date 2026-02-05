using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("Api/[controller]")]
[ApiController]
[Authorize]
public class ShippingController : ControllerBase
{
    private readonly IShippingAddressService _shippingAddressService;

    public ShippingController(IShippingAddressService shippingAddressService)
    {
        _shippingAddressService = shippingAddressService;
    }

    [HttpGet("addresses")]
    public async Task<IActionResult> GetAddresses()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated.");
        }

        var addresses = await _shippingAddressService.GetAddressesAsync(userId);
        return Ok(addresses);
    }

    [HttpPost("addresses")]
    public async Task<IActionResult> AddAddress([FromBody] CreateShippingAddressDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated.");
        }

        var address = await _shippingAddressService.AddAddressAsync(userId, dto);
        return CreatedAtAction(nameof(GetAddresses), new { }, address);
    }
}
