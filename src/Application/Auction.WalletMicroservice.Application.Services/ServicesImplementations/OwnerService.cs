using Auction.Common.Application.Models.Common;
using Auction.Common.Application.ModelsValidators;
using Auction.Common.Application.Responses;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Application.Models.Owner;
using Auction.WalletMicroservice.Application.Services.ServicesAbstractions;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Application.Services.ServicesImplementations;

public class OwnerService(
    IOwnersRepository ownersRepository,
    ITransfersRepository transfersRepository,
    IFreezingsRepository freezingsRepository,
    IModelValidator<OwnerIdModel> ownerIdValidator,
    IModelValidator<MoveMoneyModel> moveMoneyValidator,
    IMapper mapper)
        : IOwnerService
{
    private readonly IOwnersRepository _ownersRepository = ownersRepository ?? throw new ArgumentNullException(nameof(ownersRepository));
    private readonly ITransfersRepository _transfersRepository = transfersRepository ?? throw new ArgumentNullException(nameof(transfersRepository));
    private readonly IFreezingsRepository _freezingsRepository = freezingsRepository ?? throw new ArgumentNullException(nameof(freezingsRepository));

    private readonly IModelValidator<OwnerIdModel> _ownerIdValidator = ownerIdValidator ?? throw new ArgumentNullException(nameof(ownerIdValidator));
    private readonly IModelValidator<MoveMoneyModel> _moveMoneyValidator = moveMoneyValidator ?? throw new ArgumentNullException(nameof(moveMoneyValidator));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private bool _isDisposed;

    public async Task<BaseResponse> PutMoneyInWalletAsync(
        MoveMoneyModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _moveMoneyValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var owner = await _ownersRepository.GetByIdAsync(
                                model.OwnerId,
                                includeProperties: "Bill._transfersTo",
                                cancellationToken: cancellationToken);

        if (owner is null)
        {
            return BaseResponse.Error($"Не существует пользователь с Id = {model.OwnerId}");
        }

        var money = new Money(model.Money);

        var initialTransfers = owner.Bill.TransfersTo;
        owner.PutMoneyInWallet(money);
        var resultTransfers = owner.Bill.TransfersTo;

        var tasks = new List<Task>();

        foreach (var transfer in resultTransfers)
        {
            if (!initialTransfers.Contains(transfer))
            {
                var task = _transfersRepository.AddAsync(transfer, cancellationToken);
                tasks.Add(task);
            }
        }

        Task.WaitAll(tasks.ToArray(), cancellationToken);

        _ownersRepository.Update(owner);
        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success("Деньги успешно добавлены в кошелёк");
    }

    public async Task<BaseResponse> WithdrawMoneyFromWalletAsync(
        MoveMoneyModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _moveMoneyValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var owner = await _ownersRepository.GetByIdAsync(
                                model.OwnerId,
                                includeProperties: "Bill._transfersFrom",
                                cancellationToken: cancellationToken);

        if (owner is null)
        {
            return BaseResponse.Error($"Не существует пользователь с Id = {model.OwnerId}");
        }

        var money = new Money(model.Money);
        var initialTransfers = owner.Bill.TransfersFrom;

        var isWithdrawn = owner.WithdrawMoneyFromWallet(money);
        if (!isWithdrawn)
        {
            return BaseResponse.Error($"На счету недостаточно средств");
        }

        var resultTransfers = owner.Bill.TransfersFrom;
        var tasks = new List<Task>();

        foreach (var transfer in resultTransfers)
        {
            if (!initialTransfers.Contains(transfer))
            {
                var task = _transfersRepository.AddAsync(transfer, cancellationToken);
                tasks.Add(task);
            }
        }

        Task.WaitAll(tasks.ToArray(), cancellationToken);

        _ownersRepository.Update(owner);
        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success("Деньги успешно выведены на внешний счёт");
    }

    public async Task<Response<BalanceModel>> GetWalletBalanceAsync(
        OwnerIdModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _ownerIdValidator.GetErrors(model);
        if (errors is not null)
        {
            return Response<BalanceModel>.Error(errors);
        }

        var owner = await _ownersRepository.GetByIdAsync(
                            model.OwnerId,
                            includeProperties: "Bill",
                            cancellationToken: cancellationToken);

        if (owner is null)
        {
            return Response<BalanceModel>.Error($"Не существует пользователь с Id = {model.OwnerId}");
        }

        var balance = new BalanceModel(
                            owner.Bill.Money.Value,
                            owner.Bill.FrozenMoney.Value,
                            owner.Bill.FreeMoney.Value);

        return Response<BalanceModel>.Success(balance);
    }

    public async Task<Response<IList<TransactionModel>>> GetWalletTransactionsAsync(
        OwnerIdModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _ownerIdValidator.GetErrors(model);
        if (errors is not null)
        {
            return Response<IList<TransactionModel>>.Error(errors);
        }

        var owner = await _ownersRepository.GetByIdAsync(model.OwnerId, cancellationToken: cancellationToken);
        if (owner is null)
        {
            return Response<IList<TransactionModel>>.Error($"Не существует пользователь с Id = {model.OwnerId}");
        }

        var freezings = (await _freezingsRepository
            .GetAsync(
                includeProperties: "Bill.Owner",
                filter: e => e.Bill.Owner.Id == model.OwnerId,
                orderKeySelector: e => e.DateTime,
                cancellationToken: cancellationToken))
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
                    null);
            })
            .ToArray();

        var transfers = (await _transfersRepository
            .GetAsync(
                includeProperties: "FromBill.Owner, ToBill.Owner",
                filter: e => (e.FromBill != null && e.FromBill.Owner.Id == model.OwnerId)
                            || (e.ToBill != null && e.ToBill.Owner.Id == model.OwnerId),
                orderKeySelector: e => e.DateTime,
                cancellationToken: cancellationToken))
            .Select(e =>
            {
                var type = e.FromBill is null
                                ? TransactionType.PutMoney
                                : e.ToBill is null
                                    ? TransactionType.WithdrawMoney
                                    : e.ToBill.Owner.Id == model.OwnerId
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
                    _mapper.Map<LotInfoModel>(e.Lot));
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

        return Response<IList<TransactionModel>>.Success(transactions);
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _freezingsRepository.Dispose();
            _ownersRepository.Dispose();
            _transfersRepository.Dispose();

            _isDisposed = true;
        }
    }
}
