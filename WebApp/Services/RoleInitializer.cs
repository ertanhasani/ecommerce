﻿using Microsoft.AspNetCore.Identity;

namespace WebApp.Services;

public class RoleInitializer
{
    public static async Task Initialize(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var role = new IdentityRole("Admin");
            await roleManager.CreateAsync(role);
        }

        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            var role = new IdentityRole("Customer");
            await roleManager.CreateAsync(role);
        }
    }
}