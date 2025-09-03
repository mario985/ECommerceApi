using Microsoft.AspNetCore.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "User", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedAdminAsync(UserManager<User> userManager)
    {
        
        var defaultAdminEmail = "admin@example.com";
        var defaultAdminPassword = "Admin@123";
        var defaultAddress = "none";

        var adminUser = await userManager.FindByEmailAsync(defaultAdminEmail);

        if (adminUser == null)
        {
            var newAdmin = new User
            {
                UserName = defaultAdminEmail,
                Email = defaultAdminEmail,
               Address = defaultAddress
            };

            var result = await userManager.CreateAsync(newAdmin, defaultAdminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
            else
            {
                throw new Exception($"Failed to create default admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
