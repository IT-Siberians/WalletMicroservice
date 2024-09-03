using Auction.Common.Domain.RepositoriesAbstractions.Base;
using Auction.Common.Infrastructure.RepositoriesImplementations.EntityFramework;
using Auction.WalletMicroservice.Domain.Entities;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using Auction.WalletMicroservice.Infrastructure.EntityFramework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Infrastructure.RepositoriesImplementations.EntityFramework;

/// <summary>
/// UnitOfWork для EntityFramework
/// </summary>
public class EfUnitOfWork : IWalletUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

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
    /// Конструктор UnitOfWork для EntityFramework
    /// </summary>
    /// <param name="dbContext">Значение DB-контекста</param>
    /// <exception cref="ArgumentNullException">Для null-значения</exception>
    protected EfUnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        Owners = new BaseEfRepositoryWithUpdateAndDelete<ApplicationDbContext, Owner, Guid>(_dbContext);
        Bills = new BaseEfRepositoryWithUpdate<ApplicationDbContext, Bill, Guid>(_dbContext);
        Transfers = new BaseEfRepository<ApplicationDbContext, Transfer, Guid>(_dbContext);
        Freezings = new BaseEfRepository<ApplicationDbContext, Freezing, Guid>(_dbContext);
        Lots = new BaseEfRepository<ApplicationDbContext, Lot, Guid>(_dbContext);
    }

    /// <summary>
    /// Сохраняет изменения
    /// </summary>
    public void SaveChanges() => _dbContext.SaveChanges();

    /// <summary>
    /// Сохраняет изменения асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
