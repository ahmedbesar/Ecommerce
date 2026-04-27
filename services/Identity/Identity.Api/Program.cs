using Common.Logging;
using Identity.Api.Data;
using Identity.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenIddict.EntityFrameworkCore;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);
builder.ConfigureCommonLogging();

var connectionString = builder.Configuration.GetConnectionString("IdentityConnection")
    ?? throw new InvalidOperationException("Connection string IdentityConnection not found.");

var issuer = builder.Configuration["OpenIddict:Issuer"]?.TrimEnd('/')
    ?? throw new InvalidOperationException("OpenIddict:Issuer is required.");

await EnsurePostgresDatabaseExistsAsync(connectionString);

builder.Services.AddDbContext<Identity.Api.Data.ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.UseOpenIddict();
});

builder.Services.AddIdentity<Identity.Api.Entities.ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<Identity.Api.Data.ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<Identity.Api.Data.ApplicationDbContext>();
    })
    .AddServer(options =>
    {
        options.SetIssuer(new Uri(issuer));
        options.SetAuthorizationEndpointUris("connect/authorize")
            .SetTokenEndpointUris("connect/token");

        options.RegisterScopes(OpenIddict.Abstractions.OpenIddictConstants.Scopes.OpenId,
            OpenIddict.Abstractions.OpenIddictConstants.Scopes.Profile,
            OpenIddict.Abstractions.OpenIddictConstants.Scopes.Email,
            OpenIddict.Abstractions.OpenIddictConstants.Scopes.Roles,
            "ecommerce.api");
        options.RegisterAudiences("ecommerce_resource");

        if (builder.Environment.IsDevelopment())
            options.AllowPasswordFlow();

        options.AllowRefreshTokenFlow();

        options.SetAccessTokenLifetime(TimeSpan.FromHours(2));
        options.SetIdentityTokenLifetime(TimeSpan.FromMinutes(2));
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(14));

        options.DisableAccessTokenEncryption();

        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .DisableTransportSecurityRequirement();
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Identity.Api.Data.ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await Identity.Api.Data.IdentityDataSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();

static async Task EnsurePostgresDatabaseExistsAsync(string connectionString)
{
    var builderCs = new NpgsqlConnectionStringBuilder(connectionString);
    var dbName = builderCs.Database ?? throw new InvalidOperationException("Database name missing.");
    builderCs.Database = "postgres";
    await using var conn = new NpgsqlConnection(builderCs.ConnectionString);
    await conn.OpenAsync();
    await using var chk = new NpgsqlCommand(
        "SELECT 1 FROM pg_database WHERE datname = @n",
        conn);
    chk.Parameters.AddWithValue("n", dbName);
    var exists = await chk.ExecuteScalarAsync();
    if (exists is null)
    {
        var escaped = dbName.Replace("\\", "").Replace("\"", "\"\"");
        await using var create = new NpgsqlCommand($"CREATE DATABASE \"{escaped}\"", conn);
        await create.ExecuteNonQueryAsync();
    }
}



