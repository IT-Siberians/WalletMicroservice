using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Domain.RepositoriesAbstractions;

/// <summary>
/// Интерфейс UnitOfWork
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохраняет изменения
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// Сохраняет изменения асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
