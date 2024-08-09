using Auction.Common.Domain.Exceptions;

namespace Auction.Common.Domain.ValueObjects;

public abstract class ValueObject<T>
{
    public T Value { get; }

    protected ValueObject(T value, Action<T> validator)
    {
        if (value == null) throw new ArgumentNullValueException(nameof(value));
        if (validator == null) throw new ArgumentNullValueException(nameof(validator));

        validator(value);

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        return Equals(Value, ((ValueObject<T>)obj).Value);
    }

    public override int GetHashCode() => Value!.GetHashCode();

    public override string? ToString() => Value!.ToString();
}
