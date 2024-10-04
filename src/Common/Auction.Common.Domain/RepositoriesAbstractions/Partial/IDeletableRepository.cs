using Auction.Common.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Domain.RepositoriesAbstractions.Partial;

/// <summary>
/// Интерфейс репозитория для удаления сущности
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public interface IDeletableRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Удаляет сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    bool Delete(TEntity entity);

    /// <summary>
    /// Удаляет сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>true если сущность существует, иначе false</returns>
    Task<bool> DeleteByIdAsync(TKey id, CancellationToken cancellationToken);
}
