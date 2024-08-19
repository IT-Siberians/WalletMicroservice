using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Owner : AbstractPerson<Guid>
{
    public Bill Bill { get; }

    public Money Balance => Bill.Money;

    protected Owner() : base() { }

    public Owner(Guid id, Name username, Bill bill)
        : base(id, username)
    {
        Bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
    }
}
