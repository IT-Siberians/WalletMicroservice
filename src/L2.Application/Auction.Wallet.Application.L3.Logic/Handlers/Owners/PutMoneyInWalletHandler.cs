﻿using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Wallet.Application.L3.Logic.Handlers.Owners;

public class PutMoneyInWalletHandler(
    IOwnersRepository ownersRepository,
    ILotsRepository lotsRepository,
    ITransfersRepository transfersRepository,
    IFreezingsRepository freezingsRepository)
        : ICommandHandler<PutMoneyInWalletCommand>,
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

    public async Task<IAnswer> HandleAsync(PutMoneyInWalletCommand command, CancellationToken cancellationToken = default)
    {
        var owner = await _ownersRepository.GetByIdAsync(
                        command.OwnerId,
                        includeProperties: "Bill._transfersTo",
                        cancellationToken: cancellationToken);

        if (owner is null)
        {
            return BadAnswer.EntityNotFound($"Не существует пользователь с Id = {command.OwnerId}");

        }

        var money = new Money(command.Money);

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

        Task.WaitAll([.. tasks], cancellationToken);

        _ownersRepository.Update(owner);
        await _ownersRepository.SaveChangesAsync(cancellationToken);

        return new OkAnswer("Деньги успешно добавлены в кошелёк");
    }
}
