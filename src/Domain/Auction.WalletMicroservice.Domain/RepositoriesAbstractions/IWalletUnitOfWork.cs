using Auction.Common.Domain.RepositoriesAbstractions;
using Auction.Common.Domain.RepositoriesAbstractions.Base;
using Auction.WalletMicroservice.Domain.Entities;
using System;

namespace Auction.WalletMicroservice.Domain.RepositoriesAbstractions;

public interface IWalletUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Владельцы кошельков
    /// </summary>
    IBaseRepositoryWithUpdateAndDelete<Owner, Guid> Owners { get; }

    /// <summary>
    /// Кошельки
    /// </summary>
    IBaseRepositoryWithUpdate<Bill, Guid> Bills { get; }

    /// <summary>
    /// Переводы
    /// </summary>
    IBaseRepository<Transfer, Guid> Transfers { get; }

    /// <summary>
    /// Заморозки
    /// </summary>
    IBaseRepository<Freezing, Guid> Freezings { get; }

    /// <summary>
    /// Лоты
    /// </summary>
    IBaseRepository<Lot, Guid> Lots { get; }
}
