using Basket.Application.Behaviors;
using Basket.Application.Mappers;
using Basket.Core.Interfaces;
using Basket.Infrastructure.Data.Contexts;
using Basket.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.OpenApi;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Basket API",
        Version = "v1",
        Description = "This is API for Basket microservice in ecommerce application",
        Contact = new OpenApiContact
        {
            Name = "Ahmed Besar",
            Email = "besarahmed89@gmail.com",
            Url = new Uri("https://yourwebsite.eg")
        }
    });
});

// Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = builder.Configuration["CacheSettings:ConnectionString"]!;
    return ConnectionMultiplexer.Connect(connectionString);
});

// Context
builder.Services.AddSingleton<BasketContext>();

// Repositories
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Mappers
builder.Services.AddSingleton<BasketMapper>();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Basket.Application.Commands.CreateBasketCommand).Assembly);

// MediatR for CQRS with Validation Behavior
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Basket.Application.Commands.CreateBasketCommand).Assembly));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
