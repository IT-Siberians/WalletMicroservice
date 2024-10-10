using System;

namespace Auction.WalletMicroservice.Application.Models.Owner;

public record MoveMoneyModel(
    Guid OwnerId,
    decimal Money);
