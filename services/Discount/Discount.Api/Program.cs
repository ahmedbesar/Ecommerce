using Discount.Application.Behaviors;
using Discount.Application.Mappers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Grpc
builder.Services.AddGrpc();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Swagger
builder.Services.AddSwaggerGen();

// MediatR for CQRS with Validation Behavior
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Discount.Application.Commands.CreateDiscountCommand).Assembly));
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Discount.Application.Commands.CreateDiscountCommand).Assembly);

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

app.MapGrpcService<Discount.Api.Services.DiscountService>();

app.Run();
