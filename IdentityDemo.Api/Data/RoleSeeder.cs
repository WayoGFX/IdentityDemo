using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Api.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        // create roles before init the app
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = {"Admin", "User"};

        foreach (var rol in roles)
        {
            var exist = await roleManager.RoleExistsAsync(rol);
            if (!exist)
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }
    }
}