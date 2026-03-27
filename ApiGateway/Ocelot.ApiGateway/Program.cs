using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot configuration
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddJsonFile("ocelot.json", false, true);

// Add Ocelot services
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Ocelot API Gateway is running!");
    });
});

// Use Ocelot middleware
await app.UseOcelot();

app.Run();
