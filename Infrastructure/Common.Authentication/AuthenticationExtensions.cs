using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

namespace Common.Authentication;

public static class AuthenticationExtensions
{
    public const string BearerScheme = "Bearer";

    public static IServiceCollection AddEcommerceJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Authentication:Authority"]?.TrimEnd('/');
        if (string.IsNullOrEmpty(authority))
            throw new InvalidOperationException("Configuration value Authentication:Authority is required.");

        var audience = configuration["Authentication:Audience"];
        var requireHttps = configuration.GetValue("Authentication:RequireHttpsMetadata", false);

        services.AddAuthentication(BearerScheme)
            .AddJwtBearer(BearerScheme, options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.RequireHttpsMetadata = requireHttps;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIddictConstants.Claims.Name,
                    RoleClaimType = OpenIddictConstants.Claims.Role,
                    ValidateAudience = !string.IsNullOrEmpty(audience),
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}
