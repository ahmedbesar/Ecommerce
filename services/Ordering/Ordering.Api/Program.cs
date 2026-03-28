using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using System.Threading.Tasks;
using Microsoft.OpenApi;
using MassTransit;
using Ordering.Application.Consumers;
using EventBus.Messages.Common;
using Common.Logging;
using Common.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureCommonLogging();

builder.Services.AddControllers();
builder.Services.AddEcommerceJwtBearer(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ordering API",
        Version = "v1",
        Description = "This is API for Ordering microservice in ecommerce application",
        Contact = new OpenApiContact
        {
            Name = "Ahmed Besar",
            Email = "besarahmed89@gmail.com",
            Url = new Uri("https://yourwebsite.eg")
        }
    });
    options.AddSecurityDefinition(AuthenticationExtensions.BearerScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderContextSeed>>();
    await context.Database.MigrateAsync();
    await OrderContextSeed.SeedAsync(context, logger);
}

app.Run();
