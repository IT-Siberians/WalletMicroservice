using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.Common.Domain.Entities;

public abstract class AbstractPerson<TKey> : IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    public TKey Id { get; }

    public Name Username { get; protected set; }

    protected AbstractPerson() { }

    protected AbstractPerson(TKey id, Name username)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));

        Id = id;
    }

    public virtual void ChangeUsername(Name username)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
    }
}
