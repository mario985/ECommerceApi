using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { set; get; }
    public DbSet<RefreshToken> RefreshTokens { set; get; }
    public DbSet<Cart> Cart { set; get; }
    public DbSet<WishList> WishList { set; get; }
    public DbSet<Inventory> Inventories { set; get; }

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);

    //     // Explicitly set table names with correct casing
    //     builder.Entity<User>().ToTable("User");
    //     builder.Entity<RefreshToken>().ToTable("RefreshToken");
    //     builder.Entity<Cart>().ToTable("Cart");
    //     builder.Entity<WishList>().ToTable("WishList");
    //     builder.Entity<Inventory>().ToTable("Inventory");
    // }
}




public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // ⚠️ Replace with your connection string
        optionsBuilder.UseSqlite("Data Source=app.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}
