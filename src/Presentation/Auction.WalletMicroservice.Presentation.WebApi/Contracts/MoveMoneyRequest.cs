using System;

namespace Auction.WalletMicroservice.Presentation.WebApi.Contracts;

public record MoveMoneyRequest(
    Guid OwnerId,
    decimal Money);
