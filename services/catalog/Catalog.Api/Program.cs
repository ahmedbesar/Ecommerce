using Catalog.Application.Behaviors;
using Catalog.Application.Mappers;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data.Contexts;
using Catalog.Infrastructure.Repositories;
using Common.Authentication;
using Common.Logging;
using FluentValidation;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureCommonLogging();

builder.Services.AddControllers();
builder.Services.AddEcommerceJwtBearer(builder.Configuration);

builder.Services.AddOpenApi();

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
    options.AddSecurityDefinition(AuthenticationExtensions.BearerScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
});

builder.Services.AddSingleton<ICatalogContext, CatalogContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ITypeRepository, TypeRepository>();

builder.Services.AddSingleton<ProductMapper>();
builder.Services.AddSingleton<BrandMapper>();
builder.Services.AddSingleton<TypeMapper>();

builder.Services.AddValidatorsFromAssembly(typeof(Catalog.Application.Commands.CreateProductCommand).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Catalog.Application.Commands.CreateProductCommand).Assembly));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
