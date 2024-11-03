using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Application.L2.Interfaces.Repositories.Partial;

/// <summary>
/// Интерфейс репозитория для сохранения изменений
/// </summary>
public interface ISaveableRepository
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
