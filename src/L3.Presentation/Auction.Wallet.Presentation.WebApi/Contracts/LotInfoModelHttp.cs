using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts;

public record LotInfoModelHttp(
    Guid Id,
    string Title,
    string Description);
