using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Pages;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Wallet.Application.L1.Models.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Wallet.Application.L3.Logic.Handlers.Owners;

public class GetWalletTransactionsHandler(
    IOwnersRepository ownersRepository,
    ITransfersRepository transfersRepository,
    IFreezingsRepository freezingsRepository,
    IMapper mapper)
        : IQueryPageHandler<GetWalletTransactionsQuery, TransactionModel>,
        IDisposable
{
    private readonly IOwnersRepository _ownersRepository = ownersRepository ?? throw new ArgumentNullException(nameof(ownersRepository));
    private readonly ITransfersRepository _transfersRepository = transfersRepository ?? throw new ArgumentNullException(nameof(transfersRepository));
    private readonly IFreezingsRepository _freezingsRepository = freezingsRepository ?? throw new ArgumentNullException(nameof(freezingsRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private bool _isDisposed;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _freezingsRepository.Dispose();
            _ownersRepository.Dispose();
            _transfersRepository.Dispose();

            _isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async Task<IAnswer<IPageOf<TransactionModel>>> HandleAsync(GetWalletTransactionsQuery query, CancellationToken cancellationToken = default)
    {
        var owner = await _ownersRepository.GetByIdAsync(query.OwnerId, cancellationToken: cancellationToken);
        if (owner is null)
        {
            return BadAnswer<IPageOf<TransactionModel>>.EntityNotFound(CommonMessages.DoesntExistWithId, CommonNames.User, query.OwnerId);
        }

        var freezings = (await _freezingsRepository
            .GetAsync(
                includeProperties: "Bill.Owner, Lot",
                filters: [e => e.Bill.Owner.Id == query.OwnerId],
                orderKeySelector: e => e.DateTime,
                cancellationToken: cancellationToken))
            .Items
            .Select(e =>
            {
                var type = e.IsUnfreezing
                    ? TransactionType.RealeaseMoney
                    : TransactionType.ReserveMoney;
                var freezingMoney = e.IsUnfreezing
                    ? e.Money.Value
                    : -e.Money.Value;
                return new TransactionModel(
                    null,
                    freezingMoney,
                    e.DateTime,
                    type,
                    _mapper.Map<LotInfoModel>(e.Lot));
            })
            .ToArray();

        var transfers = (await _transfersRepository
            .GetAsync(
                includeProperties: "FromBill.Owner, ToBill.Owner, Lot",
                filters:
                [
                    e => e.FromBill != null && e.FromBill.Owner.Id == query.OwnerId
                            || e.ToBill != null && e.ToBill.Owner.Id == query.OwnerId
                ],
                orderKeySelector: e => e.DateTime,
                cancellationToken: cancellationToken))
            .Items
            .Select(e =>
            {
                var type = e.FromBill is null
                                ? TransactionType.PutMoney
                                : e.ToBill is null
                                    ? TransactionType.WithdrawMoney
                                    : e.ToBill.Owner.Id == query.OwnerId
                                        ? TransactionType.ReceivePaymentForLot
                                        : TransactionType.PayForLot;
                var money = type == TransactionType.PutMoney
                            || type == TransactionType.ReceivePaymentForLot
                    ? e.Money.Value
                    : -e.Money.Value;
                return new TransactionModel(
                    money,
                    null,
                    e.DateTime,
                    type,
                    e.Lot is null ? null : _mapper.Map<LotInfoModel>(e.Lot));
            })
            .ToArray();

        var transactions = new List<TransactionModel>(freezings.Length + transfers.Length);
        transactions.AddRange(transfers);
        transactions.AddRange(freezings);

        decimal frozenMoney = 0;
        decimal freeMoney = 0;

        transactions = transactions
            .OrderBy(t => t.DateTime)
            .ToList();

        transactions
            .ForEach(t =>
            {
                if (t.Type == TransactionType.PutMoney
                    || t.Type == TransactionType.WithdrawMoney
                    || t.Type == TransactionType.ReceivePaymentForLot)
                {
                    freeMoney += t.TransferMoney!.Value;
                }
                else if (t.Type == TransactionType.PayForLot)
                {
                    frozenMoney += t.TransferMoney!.Value;
                }
                else if (t.Type == TransactionType.ReserveMoney
                        || t.Type == TransactionType.RealeaseMoney)
                {
                    freeMoney += t.FreezingMoney!.Value;
                    frozenMoney -= t.FreezingMoney!.Value;
                }

                var allMoney = frozenMoney + freeMoney;

                t.Balance = new BalanceModel(allMoney, frozenMoney, freeMoney);
            });

        transactions = transactions
            .OrderByDescending(t => t.DateTime)
            .ToList();

        var page = new PageOf<TransactionModel>(
                            ItemsCount: transactions.Count,
                            PageItemsCount: transactions.Count,
                            PagesCount: 1,
                            PageNumber: 1,
                            Items: transactions);

        return new OkAnswer<IPageOf<TransactionModel>>(page);
    }
}
