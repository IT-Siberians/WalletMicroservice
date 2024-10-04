namespace Auction.Common.Domain.RepositoriesAbstractions.Partial;

/// <summary>
/// Интерфейс репозитория для обновления
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public interface IUpdatableRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Обновляет состояние сущности
    /// </summary>
    /// <param name="entity">Сущность</param>
    bool Update(TEntity entity);
}
