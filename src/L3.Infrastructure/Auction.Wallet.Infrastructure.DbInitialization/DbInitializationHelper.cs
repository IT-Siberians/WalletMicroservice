using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.Common.Domain.ValueObjects.String;
using Auction.Wallet.Infrastructure.EntityFramework;
using Auction.WalletMicroservice.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Auction.Wallet.Infrastructure.DbInitialization;

public static class DbInitializationHelper
{
    public static async Task<Owner> CreateOwnerAsync(
        this ApplicationDbContext dbContext,
        string username,
        string guid)
    {
        var owner = new Owner(new Guid(guid), new Username(username));
        await dbContext.Owners.AddAsync(owner);
        return owner;
    }

    public static async Task<Lot> CreateLotAsync(
        this ApplicationDbContext dbContext,
        string title,
        string description,
        string guid)
    {
        var lot = new Lot(new Guid(guid), new Title(title), new Text(description));
        await dbContext.Lots.AddAsync(lot);
        return lot;
    }

    public static async Task PutMoneyInWalletAsync(
        this ApplicationDbContext dbContext,
        Owner owner,
        decimal moneyValue)
    {
        var money = new Money(moneyValue);
        var transer = new Transfer(Guid.NewGuid(), money, null, owner.Bill);
        owner.Bill.PutMoney(money);
        await dbContext.Transfers.AddAsync(transer);
        await dbContext.SaveChangesAsync();
    }

    public static async Task WithdrawMoneyFromWalletAsync(
        this ApplicationDbContext dbContext,
        Owner owner,
        decimal moneyValue)
    {
        var money = new Money(moneyValue);
        var transer = new Transfer(Guid.NewGuid(), money, owner.Bill, null);
        owner.Bill.WithdrawMoney(money);
        await dbContext.Transfers.AddAsync(transer);
        await dbContext.SaveChangesAsync();
    }

    public static async Task PayForLotAsync(
        this ApplicationDbContext dbContext,
        Owner seller,
        Owner buyer,
        Lot lot,
        decimal priceValue)
    {
        var price = new Price(priceValue);
        var transer = new Transfer(Guid.NewGuid(), price, buyer.Bill, seller.Bill, lot);
        buyer.Bill.PayForLot(price);
        seller.Bill.PutMoney(price);
        await dbContext.Transfers.AddAsync(transer);
        await dbContext.SaveChangesAsync();
    }

    public static async Task ReserveMoneyAsync(
        this ApplicationDbContext dbContext,
        Owner buyer,
        Lot lot,
        decimal priceValue)
    {
        var price = new Price(priceValue);
        var freezing = new Freezing(Guid.NewGuid(), buyer.Bill, price, lot, isUnfreezing: false);
        buyer.Bill.ReserveMoney(price);
        await dbContext.Freezings.AddAsync(freezing);
        await dbContext.SaveChangesAsync();
    }

    public static async Task RealeaseMoneyAsync(
        this ApplicationDbContext dbContext,
        Owner buyer,
        Lot lot,
        decimal priceValue)
    {
        var price = new Price(priceValue);
        var freezing = new Freezing(Guid.NewGuid(), buyer.Bill, price, lot, isUnfreezing: true);
        buyer.Bill.RealeaseMoney(price);
        await dbContext.Freezings.AddAsync(freezing);
        await dbContext.SaveChangesAsync();
    }
}
