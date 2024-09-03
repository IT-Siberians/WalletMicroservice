using Auction.Common.Domain.RepositoriesAbstractions.Base;
using Auction.Common.Infrastructure.RepositoriesImplementations.InMemory;
using Auction.WalletMicroservice.Domain.Entities;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Infrastructure.RepositoriesImplementations.InMemory;

/// <summary>
/// UnitOfWork для InMemory репозиториев
/// </summary>
public class MemoryUnitOfWork : IWalletUnitOfWork
{
    /// <summary>
    /// Владельцы кошельков
    /// </summary>
    public IBaseRepositoryWithUpdateAndDelete<Owner, Guid> Owners { get; }

    /// <summary>
    /// Кошельки
    /// </summary>
    public IBaseRepositoryWithUpdate<Bill, Guid> Bills { get; }

    /// <summary>
    /// Переводы
    /// </summary>
    public IBaseRepository<Transfer, Guid> Transfers { get; }

    /// <summary>
    /// Заморозки
    /// </summary>
    public IBaseRepository<Freezing, Guid> Freezings { get; }

    /// <summary>
    /// Лоты
    /// </summary>
    public IBaseRepository<Lot, Guid> Lots { get; }

    /// <summary>
    /// Конструктор UnitOfWork для InMemory репозиториев
    /// </summary>
    protected MemoryUnitOfWork(
        IList<Owner> owners,
        IList<Bill> bills,
        IList<Transfer> transfers,
        IList<Freezing> freezings,
        IList<Lot> lots)
    {
        ArgumentNullException.ThrowIfNull(owners, nameof(owners));
        ArgumentNullException.ThrowIfNull(bills, nameof(bills));
        ArgumentNullException.ThrowIfNull(transfers, nameof(transfers));
        ArgumentNullException.ThrowIfNull(freezings, nameof(freezings));
        ArgumentNullException.ThrowIfNull(lots, nameof(lots));

        Owners = new BaseMemoryRepositoryWithUpdateAndDelete<Owner, Guid>(owners);
        Bills = new BaseMemoryRepositoryWithUpdate<Bill, Guid>(bills);
        Transfers = new BaseMemoryRepository<Transfer, Guid>(transfers);
        Freezings = new BaseMemoryRepository<Freezing, Guid>(freezings);
        Lots = new BaseMemoryRepository<Lot, Guid>(lots);
    }

    /// <summary>
    /// Сохраняет изменения
    /// </summary>
    public void SaveChanges() { }

    /// <summary>
    /// Сохраняет изменения асинхронно
    /// </summary>
    public Task SaveChangesAsync(CancellationToken _ = default)
        => Task.CompletedTask;
}
