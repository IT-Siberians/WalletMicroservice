using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Infrastructure.DbInitialization;
using Auction.Common.Presentation.Mapping;
using Auction.Common.Presentation.Validation;
using Auction.Wallet.Application.L1.Models.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Trading;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using Auction.Wallet.Application.L3.Logic.Handlers.Owners;
using Auction.Wallet.Application.L3.Logic.Handlers.Persons;
using Auction.Wallet.Application.L3.Logic.Handlers.Trading;
using Auction.Wallet.Application.L3.Logic.Mapping;
using Auction.Wallet.Infrastructure.DbInitialization;
using Auction.Wallet.Infrastructure.EntityFramework;
using Auction.Wallet.Infrastructure.Repositories.EntityFramework;
using Auction.Wallet.Presentation.GrpcApi.Services;
using Auction.Wallet.Presentation.MassTransit.Persons;
using Auction.Wallet.Presentation.Validation.Trading;
using Auction.Wallet.Presentation.WebApi.Mapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Otus.QueueDto.User;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole();

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(dbConnectionString))
{
    throw new InvalidOperationException("Connection string for ApplicationDbContext is not configured.");
}

var rmqConnectionString = builder.Configuration.GetConnectionString("RabbitMqConfig");
if (string.IsNullOrEmpty(rmqConnectionString))
{
    throw new InvalidOperationException("Connection string for RabbitMQ is not configured.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        dbConnectionString,
        opt => opt.MigrationsAssembly("Auction.Wallet.Infrastructure.EntityFramework")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Auction Wallet API",
        Description = "API of wallet microservice for purchase/sale/freeze operations and viewing of transaction history"
    }));

builder.Services.AddValidatorsFromAssemblyContaining<CreatePersonCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PayForLotCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IBillsRepository, BillsRepository>();
builder.Services.AddScoped<IFreezingsRepository, FreezingsRepository>();
builder.Services.AddScoped<ILotsRepository, LotsRepository>();
builder.Services.AddScoped<IOwnersRepository, OwnersRepository>();
builder.Services.AddScoped<ITransfersRepository, TransfersRepository>();

builder.Services.AddTransient<ICommandHandler<CreatePersonCommand>, CreatePersonHandler>();
builder.Services.AddTransient<ICommandHandler<DeletePersonCommand>, DeletePersonHandler>();
builder.Services.AddTransient<ICommandHandler<IsPersonCommand>, IsPersonHandler>();

builder.Services.AddTransient<ICommandHandler<PayForLotCommand>, PayForLotHandler>();
builder.Services.AddTransient<ICommandHandler<RealeaseMoneyCommand>, RealeaseMoneyHandler>();
builder.Services.AddTransient<ICommandHandler<ReserveMoneyCommand>, ReserveMoneyHandler>();

builder.Services.AddTransient<ICommandHandler<PutMoneyInWalletCommand>, PutMoneyInWalletHandler>();
builder.Services.AddTransient<ICommandHandler<WithdrawMoneyFromWalletCommand>, WithdrawMoneyFromWalletHandler>();

builder.Services.AddTransient<IQueryHandler<GetWalletBalanceQuery, BalanceModel>, GetWalletBalanceHandler>();

builder.Services.AddTransient<IQueryPageHandler<GetWalletTransactionsQuery, TransactionModel>, GetWalletTransactionsHandler>();

builder.Services.AddTransient<DbInitializer>();

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnectionString)
    .AddRabbitMQ(rabbitConnectionString: rmqConnectionString)
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddAutoMapper(
    typeof(ApplicationMappingProfile),
    typeof(CommonWebApiMappingProfile),
    typeof(WebApiMappingProfile));

builder.Services.AddGrpc();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateUserConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(rmqConnectionString));
        cfg.ReceiveEndpoint($"{nameof(CreateUserEvent)}.Wallet", e =>
        {
            e.ConfigureConsumer<CreateUserConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
        cfg.UseMessageRetry(r =>
        {
            r.Interval(3, TimeSpan.FromSeconds(10));
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGrpcService<TradingService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
{
    policy
        //.WithOrigins("http://localhost:3000")
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.MigrateAsync<ApplicationDbContext>();

if (app.Environment.IsDevelopment())
{
    await app.InitAsync<DbInitializer>();
}

app.Run();
