using System.Collections.Generic;
using Identity.Api.Data;
using Identity.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Data;

public static class IdentityDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var scopeManager = services.GetRequiredService<IOpenIddictScopeManager>();
        if (await scopeManager.FindByNameAsync("ecommerce.api") is null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = "ecommerce.api",
                DisplayName = "Ecommerce API access",
                Resources = { "ecommerce_resource" }
            });
        }

        var appManager = services.GetRequiredService<IOpenIddictApplicationManager>();
        var existingClient = await appManager.FindByClientIdAsync("ecommerce_dev");
        if (existingClient is null)
        {
            await appManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "ecommerce_dev",
                ClientSecret = "dev-secret-change-me",
                DisplayName = "Ecommerce development client",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.Password,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.Prefixes.Scope + "ecommerce.api",
                    Permissions.Prefixes.Scope + Scopes.OpenId,
                    Permissions.Prefixes.Scope + Scopes.Profile,
                    Permissions.Prefixes.Scope + Scopes.Email,
                    Permissions.Prefixes.Scope + Scopes.Roles,
                    Permissions.Prefixes.Scope + Scopes.OfflineAccess
                }
            });
        }
        else
        {
            // Ensure existing client has offline_access
            var descriptor = new OpenIddictApplicationDescriptor();
            await appManager.PopulateAsync(descriptor, existingClient);
            if (!descriptor.Permissions.Contains(Permissions.Prefixes.Scope + Scopes.OfflineAccess))
            {
                descriptor.Permissions.Add(Permissions.Prefixes.Scope + Scopes.OfflineAccess);
                await appManager.UpdateAsync(existingClient, descriptor);
            }
        }

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { "Admin", "User" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        if (await userManager.FindByNameAsync("bob") is null)
        {
            var u = new ApplicationUser { UserName = "bob", Email = "bob@local.test", EmailConfirmed = true };
            await userManager.CreateAsync(u, "User123!");
            await userManager.AddToRoleAsync(u, "User");
        }

        if (await userManager.FindByNameAsync("admin") is null)
        {
            var u = new ApplicationUser { UserName = "admin", Email = "admin@local.test", EmailConfirmed = true };
            await userManager.CreateAsync(u, "Admin123!");
            await userManager.AddToRoleAsync(u, "Admin");
        }
    }
}
