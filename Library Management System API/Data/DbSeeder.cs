using Library_Management_System_API.Models;

namespace Library_Management_System_API.Data;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(AppDbContext context)
    {
        if (!context.Users.Any(u => u.Role == "Admin"))
        {
            var admin = new User
            {
                Username = "Admin",
                Email = "admin@library.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Role = "Admin"
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}   