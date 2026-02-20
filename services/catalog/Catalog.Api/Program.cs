using Catalog.Application.Behaviors;
using Catalog.Application.Mappers;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data.Contexts;
using Catalog.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.OpenApi;
using System.Reflection;

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
        Title = "Catalog API",
        Version = "v1",
        Description = "This is API for Catalog microservice in ecommerce application",
        Contact = new OpenApiContact
        {
            Name = "Ahmed Besar",
            Email = "besarahmed89@gmail.com",
            Url = new Uri("https://yourwebsite.eg")
        }
    });
});

// MongoDB Context
builder.Services.AddSingleton<ICatalogContext, CatalogContext>();

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, ProductRepository>();
builder.Services.AddScoped<ITypeRepository, ProductRepository>();

// Mappers
builder.Services.AddSingleton<ProductMapper>();
builder.Services.AddSingleton<BrandMapper>();
builder.Services.AddSingleton<TypeMapper>();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Catalog.Application.Commands.CreateProductCommand).Assembly);

// MediatR for CQRS with Validation Behavior
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Catalog.Application.Commands.CreateProductCommand).Assembly));
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
