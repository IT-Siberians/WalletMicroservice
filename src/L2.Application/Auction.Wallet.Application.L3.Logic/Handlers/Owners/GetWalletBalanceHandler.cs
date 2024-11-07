using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Wallet.Application.L1.Models.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Wallet.Application.L3.Logic.Handlers.Owners;

public class GetWalletBalanceHandler(
    IOwnersRepository ownersRepository)
        : IQueryHandler<GetWalletBalanceQuery, BalanceModel>,
        IDisposable
{
    private readonly IOwnersRepository _ownersRepository = ownersRepository ?? throw new ArgumentNullException(nameof(ownersRepository));

    private bool _isDisposed;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _ownersRepository.Dispose();

            _isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async Task<IAnswer<BalanceModel>> HandleAsync(GetWalletBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var owner = await _ownersRepository.GetByIdAsync(
                            query.OwnerId,
                            includeProperties: "Bill",
                            cancellationToken: cancellationToken);

        if (owner is null)
        {
            return BadAnswer<BalanceModel>.EntityNotFound(CommonMessages.DoesntExistWithId, CommonNames.User, query.OwnerId);
        }

        var balance = new BalanceModel(
                            owner.Bill.Money.Value,
                            owner.Bill.FrozenMoney.Value,
                            owner.Bill.FreeMoney.Value);

        return new OkAnswer<BalanceModel>(balance);
    }
}
