using Auction.Common.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.Common.Domain.RepositoriesAbstractions;

/// <summary>
/// Базовый интерфейс репозитория
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип уникального идетификатора сущности</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Возвращает все сущности
    /// </summary>
    /// <returns>Перечисление сущностей</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Возвращает сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Значение уникального идентификатора</param>
    /// <returns>Найденная сущность или null</returns>
    Task<TEntity?> GetByIdAsync(TKey id);

    /// <summary>
    /// Добавляет новую сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Обновляет состояние сущности
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность существует и ее удалось обновить, иначе false</returns>
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// Удаляет сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность существует и ее удалось удалить, иначе false</returns>
    Task<bool> DeleteAsync(TEntity entity);

    /// <summary>
    /// Удаляет сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <returns>true если сущность существует и ее удалось удалить, иначе false</returns>
    Task<bool> DeleteAsync(TKey id);
}
