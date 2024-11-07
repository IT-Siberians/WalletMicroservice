using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Owner;

public record LotInfoModelWeb(
    Guid Id,
    string Title,
    string Description);
