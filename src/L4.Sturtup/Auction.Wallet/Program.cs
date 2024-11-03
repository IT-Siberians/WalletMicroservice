using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Presentation.Initialization;
using Auction.Common.Presentation.Mapping;
using Auction.Common.Presentation.Validation;
using Auction.Wallet;
using Auction.Wallet.Application.L1.Models.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Traiding;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using Auction.Wallet.Application.L3.Logic.Handlers.Owners;
using Auction.Wallet.Application.L3.Logic.Handlers.Persons;
using Auction.Wallet.Application.L3.Logic.Handlers.Traiding;
using Auction.Wallet.Application.L3.Logic.Mapping;
using Auction.Wallet.Infrastructure.EntityFramework;
using Auction.Wallet.Infrastructure.Repositories.EntityFramework;
using Auction.Wallet.Presentation.Validation.Traiding;
using Auction.Wallet.Presentation.WebApi.Mapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(dbConnectionString))
{
    throw new InvalidOperationException("Connection string for ApplicationDbContext is not configured.");
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

builder.Services.AddAutoMapper(
    typeof(ApplicationMappingProfile),
    typeof(CommonPresentationMappingProfile),
    typeof(PresentationMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.MigrateAsync<ApplicationDbContext>();

if (app.Environment.IsDevelopment())
{
    await app.InitAsync<DbInitializer>();
}

app.Run();
