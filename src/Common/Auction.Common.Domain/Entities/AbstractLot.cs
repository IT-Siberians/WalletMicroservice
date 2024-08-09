using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;

namespace Auction.Common.Domain.Entities;

public abstract class AbstractLot<TKey> : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; protected set; }

    protected Name? _title;
    protected Text? _description;

    public Name Title => _title ?? throw new FieldNullValueException(nameof(_title));
    public Text Description => _description ?? throw new FieldNullValueException(nameof(_description));

    protected AbstractLot(TKey id)
    {
        Id = id;
    }

    protected AbstractLot(TKey id, Name title, Text description)
    {
        _title = title ?? throw new ArgumentNullValueException(nameof(title));
        _description = description ?? throw new ArgumentNullValueException(nameof(description));

        Id = id;
    }
}
