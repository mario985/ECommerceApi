using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Stripe;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;

public class CustomWebApplicationFactory 
    : WebApplicationFactory<Program>
{
    private SqliteConnection _connection;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove original DbContext
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

            // Create SQLite in-memory connection
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            // Build provider and create schema
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "Test", options => { });
        });
      
builder.ConfigureAppConfiguration((context, config) =>
{
    var settings = new Dictionary<string, string>
    {
        {"MongoDb:ConnectionURI", "mongodb://localhost:27017"},
        {"MongoDb:DatabaseName", "TestDatabase"}
    };

    config.AddInMemoryCollection(settings);
});

    builder.ConfigureServices(services =>
    {
        // Remove the production IMongoDatabase registration
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(IMongoDatabase));

        if (descriptor != null)
            services.Remove(descriptor);

        // Re-register with test values hardcoded or from test config
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = new MongoClient("mongodb://localhost:27017");
            return client.GetDatabase("TestDatabase");
        });
    });
}
}