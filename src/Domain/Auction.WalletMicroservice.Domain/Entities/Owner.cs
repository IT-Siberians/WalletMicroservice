using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Owner : AbstractPerson<Guid>
{
    private Bill? _bill;

    public Bill Bill => _bill ?? throw new FieldNullValueException(nameof(_bill));
    public Money Balance => Bill.Money;

    protected Owner() : base(Guid.Empty) { }

    public Owner(Guid id, Name username, Bill bill)
        : base(id, username)
    {
        _bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
    }
}
