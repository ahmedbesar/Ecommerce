using Discount.Api.Services;
using Discount.Application.Behaviors;
using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Common.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureCommonLogging();

// Add services to the container.
builder.Services.AddControllers();

// Grpc
builder.Services.AddGrpc();

// Kestrel Configuration - separate ports for REST and gRPC
builder.WebHost.ConfigureKestrel(options =>
{
    // REST/Swagger endpoint (HTTP/1.1)
    options.ListenAnyIP(8002, o => o.Protocols = HttpProtocols.Http1);
    // gRPC endpoint (HTTP/2 cleartext)
    options.ListenAnyIP(5002, o => o.Protocols = HttpProtocols.Http2);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Swagger
builder.Services.AddSwaggerGen();

// MediatR for CQRS with Validation Behavior
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDiscountCommand).Assembly));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateDiscountCommand).Assembly);

// Mappers
builder.Services.AddSingleton<DiscountMapper>();

// Repositories
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var app = builder.Build();

// Migrate Database
await app.MigrateDatabaseAsync<Program>();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<DiscountService>();

app.Run();
