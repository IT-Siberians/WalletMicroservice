using Auction.Common.Domain.Exceptions;

namespace Auction.Common.Domain.ValueObjects;

public class Text(string value)
    : ValueObject<string>(
        value,
        value =>
        {
            if (value == null) throw new TextNullValueException();
            if (string.IsNullOrWhiteSpace(value)) throw new TextEmptyValueException(value);
        });
