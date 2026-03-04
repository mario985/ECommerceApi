using Microsoft.EntityFrameworkCore;

public static class TestDataSeeder
{

    public static async Task<string> SeedUserAsync(AppDbContext context)
    {
        var user = new User
        {
            Id = "test-user-id",
            UserName = "testuser",
            Email = "test@test.com",
        };
        
        var dbUser =await  context.Users.FirstOrDefaultAsync(u =>u.Id ==user.Id);
        if (dbUser != null)
        {
            return dbUser.Id;
        }
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.Id;
    }
}