using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.Common.Domain.Entities;

public abstract class AbstractPerson<TKey> : IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    public TKey Id { get; protected set; }

    protected Name? _username;

    public Name Username => _username ?? throw new FieldNullValueException(nameof(_username));

    protected AbstractPerson(TKey id)
    {
        Id = id;
    }

    protected AbstractPerson(TKey id, Name username)
    {
        _username = username ?? throw new ArgumentNullValueException(nameof(username));

        Id = id;
    }

    public virtual void ChangeUsername(Name username)
    {
        _username = username ?? throw new ArgumentNullValueException(nameof(username));
    }
}
