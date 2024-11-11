using Auction.Common.Infrastructure.DbInitialization;
using Auction.Wallet.Infrastructure.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Wallet.Infrastructure.DbInitialization;

public class DbInitializer(ApplicationDbContext dbContext) : IDbInitializer
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    private bool _isDisposed;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _dbContext.Dispose();

            _isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async Task InitDatabaseAsync()
    {
        const string person1IdString = "11111111-5717-4562-b3fc-111100001111";
        Guid person1Id = new(person1IdString);

        if (_dbContext.Owners.Any(p => p.Id == person1Id))
        {
            return;
        }

        await InitAsync(person1IdString);
    }

    private async Task InitAsync(string person1IdString)
    {
        var owner1 = await _dbContext.CreateOwnerAsync("AlexA", person1IdString);
        var owner2 = await _dbContext.CreateOwnerAsync("BorisB", "11111111-5717-4562-b3fc-111100002222");
        var owner3 = await _dbContext.CreateOwnerAsync("CarinC", "11111111-5717-4562-b3fc-111100003333");
        var owner4 = await _dbContext.CreateOwnerAsync("DenisD", "11111111-5717-4562-b3fc-111100004444");

        _ = await _dbContext.CreateLotAsync(
                title: "Первый невыкупленный лот",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200001111");
        _ = await _dbContext.CreateLotAsync(
                title: "Второй невыкупленный лот",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200002222");
        _ = await _dbContext.CreateLotAsync(
                title: "Третий невыкупленный лот",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200003333");
        _ = await _dbContext.CreateLotAsync(
                title: "Четвёртый невыкупленный лот",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200004444");

        _ = await _dbContext.CreateLotAsync(
                title: "Отменённый лот 1го продавца",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200005555");
        _ = await _dbContext.CreateLotAsync(
                title: "Отменённый лот 4го продавца",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200006666");

        var lot7 = await _dbContext.CreateLotAsync(
                title: "Первый выкупленный лот первого продавца",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200007777");
        var lot8 = await _dbContext.CreateLotAsync(
                title: "Второй выкупленный лот первого продавца",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200008888");
        var lot9 = await _dbContext.CreateLotAsync(
                title: "Выкупленный лот 3го продавца",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-222200009999");
        var lot10 = await _dbContext.CreateLotAsync(
                title: "Выкупленный лот 4го продавца",
                description: "Описание лота",
                guid: "22222222-5717-4562-b3fc-22220000aaaa");

        await _dbContext.SaveChangesAsync();

        await _dbContext.PutMoneyInWalletAsync(owner1, 3000);
        await _dbContext.PutMoneyInWalletAsync(owner2, 5000);
        await _dbContext.PutMoneyInWalletAsync(owner3, 45000);
        await _dbContext.PutMoneyInWalletAsync(owner4, 5000);

        await _dbContext.ReserveMoneyAsync(owner2, lot7, 3500);
        await _dbContext.PayForLotAsync(owner1, owner2, lot7, 3500);

        await _dbContext.ReserveMoneyAsync(owner3, lot8, 10000);
        await _dbContext.PutMoneyInWalletAsync(owner4, 10000);
        await _dbContext.RealeaseMoneyAsync(owner3, lot8, 10000);
        await _dbContext.ReserveMoneyAsync(owner4, lot8, 11000);
        await _dbContext.RealeaseMoneyAsync(owner4, lot8, 11000);
        await _dbContext.ReserveMoneyAsync(owner3, lot8, 33000);
        await _dbContext.PayForLotAsync(owner1, owner3, lot8, 33000);

        await _dbContext.ReserveMoneyAsync(owner4, lot9, 3500);
        await _dbContext.PayForLotAsync(owner3, owner4, lot9, 3500);

        await _dbContext.ReserveMoneyAsync(owner3, lot10, 2500);
        await _dbContext.PayForLotAsync(owner4, owner3, lot10, 2500);

        await _dbContext.WithdrawMoneyFromWalletAsync(owner1, 20000);
    }
}

