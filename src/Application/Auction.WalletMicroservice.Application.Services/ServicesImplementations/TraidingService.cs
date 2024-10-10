using Auction.Common.Application.ModelsValidators;
using Auction.Common.Application.Responses;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.Common.Domain.ValueObjects.String;
using Auction.WalletMicroservice.Application.Models.Traiding;
using Auction.WalletMicroservice.Application.Services.ServicesAbstractions;
using Auction.WalletMicroservice.Domain.Entities;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Application.Services.ServicesImplementations;

public class TraidingService(
    IOwnersRepository ownersRepository,
    ILotsRepository lotsRepository,
    ITransfersRepository transfersRepository,
    IFreezingsRepository freezingsRepository,
    IModelValidator<ReserveMoneyModel> reserveMoneyValidator,
    IModelValidator<RealeaseMoneyModel> realeaseMoneyValidator,
    IModelValidator<PayForLotModel> payForLotValidator)
        : ITraidingService
{
    private readonly IOwnersRepository _ownersRepository = ownersRepository ?? throw new ArgumentNullException(nameof(ownersRepository));
    private readonly ILotsRepository _lotsRepository = lotsRepository ?? throw new ArgumentNullException(nameof(lotsRepository));
    private readonly ITransfersRepository _transfersRepository = transfersRepository ?? throw new ArgumentNullException(nameof(transfersRepository));
    private readonly IFreezingsRepository _freezingsRepository = freezingsRepository ?? throw new ArgumentNullException(nameof(freezingsRepository));

    private readonly IModelValidator<ReserveMoneyModel> _reserveMoneyValidator = reserveMoneyValidator ?? throw new ArgumentNullException(nameof(reserveMoneyValidator));
    private readonly IModelValidator<RealeaseMoneyModel> _realeaseMoneyValidator = realeaseMoneyValidator ?? throw new ArgumentNullException(nameof(realeaseMoneyValidator));
    private readonly IModelValidator<PayForLotModel> _payForLotValidator = payForLotValidator ?? throw new ArgumentNullException(nameof(payForLotValidator));

    private bool _isDisposed;

    public async Task<BaseResponse> ReserveMoneyAsync(
        ReserveMoneyModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _reserveMoneyValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var buyer = await _ownersRepository.GetByIdAsync(
                                model.BuyerId,
                                includeProperties: "Bill._freezings",
                                cancellationToken: cancellationToken);

        if (buyer is null)
        {
            return BaseResponse.Error($"Не существует покупатель с Id = {model.BuyerId}");
        }

        var lot = await _lotsRepository.GetByIdAsync(model.Lot.Id, cancellationToken: cancellationToken);
        if (lot is null)
        {
            lot = new Lot(
                        model.Lot.Id,
                        new Title(model.Lot.Title),
                        new Text(model.Lot.Description));

            await _lotsRepository.AddAsync(lot, cancellationToken);
            await _lotsRepository.SaveChangesAsync(cancellationToken);
        }

        var price = new Price(model.Price);
        var initialFreezings = buyer.Bill.Freezings;

        if (!buyer.ReserveMoney(price, lot))
        {
            return BaseResponse.Error("На счёте недостаточно свободных денег");
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

        Task.WaitAll(tasks.ToArray(), cancellationToken);

        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success("Деньги успешно зарезервированы");
    }

    public async Task<BaseResponse> RealeaseMoneyAsync(
        RealeaseMoneyModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _realeaseMoneyValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var buyer = await _ownersRepository.GetByIdAsync(
                                model.BuyerId,
                                includeProperties: "Bill._freezings",
                                cancellationToken: cancellationToken);

        if (buyer is null)
        {
            return BaseResponse.Error($"Не существует покупатель с Id = {model.BuyerId}");
        }

        var lot = await _lotsRepository.GetByIdAsync(model.LotId, cancellationToken: cancellationToken);
        if (lot is null)
        {
            return BaseResponse.Error($"Не существует лот с Id = {model.LotId}");
        }

        var price = new Price(model.Price);
        var initialFreezings = buyer.Bill.Freezings;

        if (!buyer.RealeaseMoney(price, lot))
        {
            return BaseResponse.Error("На счёте недостаточно зарезервированных денег");
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

        Task.WaitAll(tasks.ToArray(), cancellationToken);

        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success("Деньги успешно разморожены");
    }

    public async Task<BaseResponse> PayForLotAsync(
        PayForLotModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _payForLotValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var buyer = await _ownersRepository.GetByIdAsync(
                                model.BuyerId,
                                includeProperties: "Bill._transfersFrom",
                                cancellationToken: cancellationToken);

        if (buyer is null)
        {
            return BaseResponse.Error($"Не существует покупатель с Id = {model.BuyerId}");
        }

        var seller = await _ownersRepository.GetByIdAsync(
                                model.SellerId,
                                includeProperties: "Bill",
                                cancellationToken: cancellationToken);

        if (seller is null)
        {
            return BaseResponse.Error($"Не существует продавец с Id = {model.SellerId}");
        }

        var lot = await _lotsRepository.GetByIdAsync(model.LotId, cancellationToken: cancellationToken);
        if (lot is null)
        {
            return BaseResponse.Error($"Не существует лот с Id = {model.LotId}");
        }

        var price = new Price(model.HammerPrice);

        if (!buyer.HasFrozenMoney(price))
        {
            return BaseResponse.Error("Зарезервировано недостаточно средств");
        }

        var initialTransfers = buyer.Bill.TransfersFrom;

        if (!buyer.PayForLot(price, seller, lot))
        {
            return BaseResponse.Error("Не удалось оплатить лот");
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

        Task.WaitAll(tasks.ToArray(), cancellationToken);

        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success("Оплата лота прошла успешно");
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _ownersRepository.Dispose();
            _lotsRepository.Dispose();

            _isDisposed = true;
        }
    }
}
