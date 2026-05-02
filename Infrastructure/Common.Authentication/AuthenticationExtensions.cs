using Common.Authentication.Policies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace Common.Authentication;

public static class AuthenticationExtensions
{
    public const string BearerScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;

    public static IServiceCollection AddEcommerceJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Authentication:Authority"]?.TrimEnd('/');
        if (string.IsNullOrEmpty(authority))
            throw new InvalidOperationException("Configuration value Authentication:Authority is required.");

        var audience = configuration["Authentication:Audience"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = BearerScheme;
            options.DefaultChallengeScheme = BearerScheme;
            options.DefaultForbidScheme = BearerScheme;
        });

        services.AddOpenIddict()
            .AddValidation(options =>
            {
                // Point to the OpenIddict server (fetches JWKS from its discovery doc)
                options.SetIssuer(authority);

                if (!string.IsNullOrEmpty(audience))
                    options.AddAudiences(audience);

                // Use System.Net.Http to introspect / fetch JWKS
                options.UseSystemNetHttp();

                // Register the ASP.NET Core host
                options.UseAspNetCore();
            });

        var policies = new List<IAuthorizationPolicy>
        {
            new AdminPolicy(),
            new SelfUserOrAdminPolicy(),
        };

        services.AddAuthorization(options =>
        {
            foreach (var policy in policies)
                options.AddPolicy(policy.PolicyName, builder => policy.Apply(builder));
        });

        return services;
    }
}
