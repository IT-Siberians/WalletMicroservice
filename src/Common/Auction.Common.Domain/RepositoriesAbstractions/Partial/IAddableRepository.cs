using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Domain.RepositoriesAbstractions.Partial;

/// <summary>
/// Интерфейс репозитория для добавления новой сущности
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public interface IAddableRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Добавляет новую сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>true если сущность добавлена, иначе false</returns>
    Task<bool> AddAsync(TEntity entity, CancellationToken cancellationToken);
}
