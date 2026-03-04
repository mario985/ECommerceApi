using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ECommerceApi.IntegrationTests;

public class carttEndpointsTests 
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly IMongoCollection<Product>_productCollection;
    private readonly CustomWebApplicationFactory _factory;
    private readonly IInventoryRepository? _inventoryRepository;
    public carttEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        var scope = _factory.Services.CreateScope();
        var database = scope.ServiceProvider.GetService<IMongoDatabase>();
        _inventoryRepository = scope.ServiceProvider.GetService<IInventoryRepository>();
        _productCollection = database.GetCollection<Product>("Products");
    }
    [Fact]
    public async Task integrationtest_should_return_Ok ()
    {
        var response = await _client.GetAsync("/api/Cart/test");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    [Fact]
    public async Task Add_to_cart_should_return_Ok_when_valid()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
       var userId =  await TestDataSeeder.SeedUserAsync(context);
        var productId = await SeedProductToMongoDb();
        await seedProductToInventory(productId);
        // arrange
        var product = new AddToCartDto
        {
            userId = userId,
            productId = productId,
            Quantity = 2
        };
    
        // Act
        var response = await _client.PostAsJsonAsync("api/cart/Add" , product);
       response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        // Then
        await _productCollection.DeleteOneAsync(p =>p.Id ==product.productId);
    }
    [Fact]
     public async Task Add_to_cart_should_return_BadRequest_when_productId_is_invalid()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
       var userId =  await TestDataSeeder.SeedUserAsync(context);
        var productId = await SeedProductToMongoDb();
        // arrange
        var product = new AddToCartDto
        {
            userId = userId,
            productId = "6975ccc995fbefb85de6135a",
            Quantity = 2
        };
    
        // Act
        var response = await _client.PostAsJsonAsync("api/cart/Add" , product);
       response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        // Then
        await _productCollection.DeleteOneAsync(p =>p.Id ==product.productId);
    }
    [Fact]
public async Task Remove_from_cart_should_return_Ok_when_product_exists_in_cart()
{
    // Arrange
    using var scope = _factory.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userId = await TestDataSeeder.SeedUserAsync(context);
    var productId = await SeedProductToMongoDb();

    // First add the item to cart
    var addDto = new AddToCartDto { userId = userId, productId = productId, Quantity = 1 };
    await _client.PostAsJsonAsync("api/cart/Add", addDto);

    // Act
    var response = await _client.DeleteAsync($"api/cart/remove/{productId}");

    // Assert
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

    // Cleanup
    await _productCollection.DeleteOneAsync(p => p.Id == productId);
}
    private async Task<string> SeedProductToMongoDb()
    {
        var result = await _productCollection.Find(p =>p.Name == "Test Product").FirstOrDefaultAsync();
        if (result != null)
        {
            return result.Id;
        }
        var product = new Product
    {
        Description = "sadfas",
        Category  = ProductCategory.Electronics,
        Name = "Test Product",
        Price = 100
    };

    await _productCollection.InsertOneAsync(product);
    return product.Id;
    }
    private async Task seedProductToInventory(string productId)
    {
        var inventory = new Inventory
        {
            ProductId = productId,
            OnHand = 10
        };
        var result = await _inventoryRepository.GetByProductIdAsync(productId);
        if(result!=null)return;
        await _inventoryRepository.AddAsync(inventory);
        
    }
}