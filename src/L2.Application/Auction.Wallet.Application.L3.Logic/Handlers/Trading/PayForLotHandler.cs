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

public class PayForLotHandler(
    IOwnersRepository ownersRepository,
    ILotsRepository lotsRepository,
    ITransfersRepository transfersRepository,
    IFreezingsRepository freezingsRepository)
        : ICommandHandler<PayForLotCommand>,
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

    public async Task<IAnswer> HandleAsync(PayForLotCommand command, CancellationToken cancellationToken = default)
    {
        var buyer = await _ownersRepository.GetByIdAsync(
                                command.BuyerId,
                                includeProperties: "Bill._transfersFrom",
                                cancellationToken: cancellationToken);

        if (command.SellerId == command.BuyerId)
        {
            return BadAnswer.Error(WalletMessages.BuyerAndSellerIdsCannotMatch, command.SellerId);
        }

        if (buyer is null)
        {
            return BadAnswer.EntityNotFound(CommonMessages.DoesntExistWithId, CommonNames.Buyer, command.BuyerId);
        }

        var seller = await _ownersRepository.GetByIdAsync(
                                command.SellerId,
                                includeProperties: "Bill",
                                cancellationToken: cancellationToken);

        if (seller is null)
        {
            return BadAnswer.EntityNotFound(CommonMessages.DoesntExistWithId, CommonNames.Seller, command.SellerId);
        }

        var lot = await _lotsRepository.GetByIdAsync(command.LotId, cancellationToken: cancellationToken);
        if (lot is null)
        {
            return BadAnswer.EntityNotFound(CommonMessages.DoesntExistWithId, CommonNames.Lot, command.LotId);
        }

        var price = new Price(command.HammerPrice);

        if (!buyer.HasFrozenMoney(price))
        {
            return BadAnswer.Error(WalletMessages.NotEnoughMoneyReserved);
        }

        var initialTransfers = buyer.Bill.TransfersFrom;

        if (!buyer.PayForLot(price, seller, lot))
        {
            return BadAnswer.Error(WalletMessages.FailedToPayForLot);
        }

        var resultTransfers = buyer.Bill.TransfersFrom;
        var tasks = new List<Task>();

        foreach (var transfer in resultTransfers)
        {
            if (!initialTransfers.Contains(transfer))
            {
                var task = _transfersRepository.AddAsync(transfer, cancellationToken);
                tasks.Add(task);
            }
        }

        Task.WaitAll([.. tasks], cancellationToken);

        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return new OkAnswer(WalletMessages.PaymentForLotWasSuccessful);
    }
}
