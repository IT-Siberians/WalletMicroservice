namespace Auction.Common.Domain.Entities;

/// <summary>
/// Интерфес мягкого удаления сущности
/// </summary>
public interface IDeletableSoftly
{
    /// <summary>
    /// Является ли пользователь удалённым
    /// </summary>
    bool IsDeletedSoftly { get; }

    /// <summary>
    /// Помечает пользователя как удалённого
    /// </summary>
    void MarkAsDeletedSoftly();
}
