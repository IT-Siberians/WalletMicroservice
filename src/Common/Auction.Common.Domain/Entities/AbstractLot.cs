using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.Common.Domain.Entities;

public abstract class AbstractLot<TKey> : IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    public TKey Id { get; }
    public Name Title { get; protected set; }
    public Text Description { get; protected set; }

    protected AbstractLot() { }

    protected AbstractLot(TKey id, Name title, Text description)
    {
        Title = title ?? throw new ArgumentNullValueException(nameof(title));
        Description = description ?? throw new ArgumentNullValueException(nameof(description));

        Id = id;
    }
}
