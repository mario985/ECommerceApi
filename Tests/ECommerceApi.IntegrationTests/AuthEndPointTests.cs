using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

public class AuthEndPointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    public AuthEndPointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        var scope = _factory.Services.CreateScope();

    }
    [Fact]
    public async Task Register_ValidData_Returns200()
    {
        var body = new RegisterDto
        { Email = "test@test.com",
         Password = "Test@1234",
        UserName = "testuser" ,
        Address = "6elbasfa;lj;flasdjf" ,
        ConfirmPassword = "Test@1234" 
        };
        var response = await _client.PostAsJsonAsync("/Api/Account/Register", body);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Register_DuplicateEmail_Returns400()
    {
          var body = new RegisterDto
        { Email = "test@testt.com",
         Password = "Test@1234",
        UserName = "testtuser" ,
        Address = "6elbasfa;lj;flasdjf" ,
        ConfirmPassword = "Test@1234" 
        };
        await _client.PostAsJsonAsync("/Api/Account/Register", body); // first time
        var response = await _client.PostAsJsonAsync("/Api/Account/Register", body); // duplicate
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task Register_WeakPassword_Returns400()
    {
         var body = new RegisterDto
        { Email = "weak@testt.com",
         Password = "Test@1234",
        UserName = "weakUser" ,
        Address = "6elbasfa;lj;flasdjf" ,
        ConfirmPassword = "123" 
        };
        var response = await _client.PostAsJsonAsync("/Api/Account/Register", body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task Register_MissingFields_Returns400()
    {
        var body = new { Email = "" , Password = "", UserName = "" };
        var response = await _client.PostAsJsonAsync("/Api/Account/Register", body);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}