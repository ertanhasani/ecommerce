using Microsoft.AspNetCore.Identity;
using WebApp.Resources;

namespace WebApp.Services;

public class RoleInitializer
{
    public static async Task Initialize(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(GeneralConstant.Roles.Admin))
        {
            var role = new IdentityRole(GeneralConstant.Roles.Admin);
            await roleManager.CreateAsync(role);
        }

        if (!await roleManager.RoleExistsAsync(GeneralConstant.Roles.Customer))
        {
            var role = new IdentityRole(GeneralConstant.Roles.Customer);
            await roleManager.CreateAsync(role);
        }
    }
}