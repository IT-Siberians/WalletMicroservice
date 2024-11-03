using Auction.Common.Application.Models.Common;
using Auction.Common.Application.ModelsValidators;
using Auction.Common.Application.ServicesAbstractions;
using Auction.Common.Presentation.Initialization;
using Auction.WalletMicroservice.Application.Models.ModelsValidators;
using Auction.WalletMicroservice.Application.Models.Owner;
using Auction.WalletMicroservice.Application.Models.Traiding;
using Auction.WalletMicroservice.Application.Services.Mapping;
using Auction.WalletMicroservice.Application.Services.ServicesAbstractions;
using Auction.WalletMicroservice.Application.Services.ServicesImplementations;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using Auction.WalletMicroservice.Infrastructure.EntityFramework;
using Auction.WalletMicroservice.Infrastructure.RepositoriesImplementations.EntityFramework;
using Auction.WalletMicroservice.Presentation.WebApi.Mapping;
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
        opt => opt.MigrationsAssembly("Auction.WalletMicroservice.Infrastructure.EntityFramework")));

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

builder.Services.AddSingleton<IModelValidator<LotInfoModel>, LotInfoValidator>();
builder.Services.AddSingleton<IModelValidator<PersonInfoModel>, PersonInfoValidator>();
builder.Services.AddSingleton<IModelValidator<PersonIdModel>, PersonIdValidator>();
builder.Services.AddSingleton<IModelValidator<OwnerIdModel>, OwnerIdValidator>();
builder.Services.AddSingleton<IModelValidator<MoveMoneyModel>, MoveMoneyValidator>();
builder.Services.AddSingleton<IModelValidator<ReserveMoneyModel>, ReserveMoneyValidator>();
builder.Services.AddSingleton<IModelValidator<RealeaseMoneyModel>, RealeaseMoneyValidator>();
builder.Services.AddSingleton<IModelValidator<PayForLotModel>, PayForLotValidator>();

builder.Services.AddScoped<IBillsRepository, BillsRepository>();
builder.Services.AddScoped<IFreezingsRepository, FreezingsRepository>();
builder.Services.AddScoped<ILotsRepository, LotsRepository>();
builder.Services.AddScoped<IOwnersRepository, OwnersRepository>();
builder.Services.AddScoped<ITransfersRepository, TransfersRepository>();

builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ITraidingService, TraidingService>();

builder.Services.AddAutoMapper(
    typeof(ApplicationMappingProfile),
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

app.Run();



