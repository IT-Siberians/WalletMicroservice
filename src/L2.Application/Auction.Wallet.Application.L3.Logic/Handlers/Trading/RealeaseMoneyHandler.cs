using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.Wallet.Application.L2.Interfaces.Commands.Trading;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using Auction.Wallet.Application.L3.Logic.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Wallet.Application.L3.Logic.Handlers.Trading;

public class RealeaseMoneyHandler(
    IOwnersRepository ownersRepository,
    ILotsRepository lotsRepository,
    ITransfersRepository transfersRepository,
    IFreezingsRepository freezingsRepository)
        : ICommandHandler<RealeaseMoneyCommand>,
        IDisposable
{
    private readonly IOwnersRepository _ownersRepository = ownersRepository ?? throw new ArgumentNullException(nameof(ownersRepository));
    private readonly ILotsRepository _lotsRepository = lotsRepository ?? throw new ArgumentNullException(nameof(lotsRepository));
    private readonly ITransfersRepository _transfersRepository = transfersRepository ?? throw new ArgumentNullException(nameof(transfersRepository));
    private readonly IFreezingsRepository _freezingsRepository = freezingsRepository ?? throw new ArgumentNullException(nameof(freezingsRepository));

    private bool _isDisposed;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _ownersRepository.Dispose();
            _lotsRepository.Dispose();
            _transfersRepository.Dispose();
            _freezingsRepository.Dispose();

            _isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async Task<IAnswer> HandleAsync(RealeaseMoneyCommand command, CancellationToken cancellationToken = default)
    {
        var buyer = await _ownersRepository.GetByIdAsync(
                        command.BuyerId,
                        includeProperties: "Bill._freezings",
                        cancellationToken: cancellationToken);

        if (buyer is null)
        {
            return BadAnswer.EntityNotFound(CommonMessages.DoesntExistWithId, CommonNames.Buyer, command.BuyerId);
        }

        var lot = await _lotsRepository.GetByIdAsync(command.LotId, cancellationToken: cancellationToken);
        if (lot is null)
        {
            return BadAnswer.EntityNotFound(CommonMessages.DoesntExistWithId, CommonNames.Lot, command.LotId);
        }

        var price = new Price(command.Price);
        var initialFreezings = buyer.Bill.Freezings;

        if (!buyer.RealeaseMoney(price, lot))
        {
            return BadAnswer.Error(WalletMessages.ThereIsNotEnoughReservedMoneyInBill);
        }

        var resultFreezings = buyer.Bill.Freezings;
        var tasks = new List<Task>();

        foreach (var freezing in resultFreezings)
        {
            if (!initialFreezings.Contains(freezing))
            {
                var task = _freezingsRepository.AddAsync(freezing, cancellationToken);
                tasks.Add(task);
            }
        }

        Task.WaitAll([.. tasks], cancellationToken);

        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return new OkAnswer(WalletMessages.MoneySuccessfullyUnfrozen);
    }
}
