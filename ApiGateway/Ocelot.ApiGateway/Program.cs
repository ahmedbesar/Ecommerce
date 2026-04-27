using Common.Authentication;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddJsonFile("ocelot.json", false, true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddEcommerceJwtBearer(builder.Configuration);

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Ocelot API Gateway is running!");
    });
});

await app.UseOcelot();

app.Run();
