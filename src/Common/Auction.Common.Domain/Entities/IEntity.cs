namespace Auction.Common.Domain.Entities;

public interface IEntity<TKey> where TKey : notnull, IEquatable<TKey>
{
    public TKey Id { get; }
}
