using Basket.Application.Behaviors;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Core.Interfaces;
using Basket.Infrastructure.Data.Contexts;
using Basket.Infrastructure.Repositories;
using Common.Authentication;
using Discount.Grpc.Protos;
using FluentValidation;
using Microsoft.OpenApi;
using StackExchange.Redis;
using MassTransit;
using Common.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureCommonLogging();

builder.Services.AddControllers();
builder.Services.AddEcommerceJwtBearer(builder.Configuration);

builder.Services.AddOpenApi();

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
    options.AddSecurityDefinition(AuthenticationExtensions.BearerScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var connectionString = builder.Configuration["CacheSettings:ConnectionString"]!;
    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddSingleton<BasketContext>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddSingleton<BasketMapper>();

builder.Services.AddValidatorsFromAssembly(typeof(Basket.Application.Commands.CreateBasketCommand).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Basket.Application.Commands.CreateBasketCommand).Assembly));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});

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

