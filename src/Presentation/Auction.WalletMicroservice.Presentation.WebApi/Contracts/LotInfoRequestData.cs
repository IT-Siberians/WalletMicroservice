using System;

namespace Auction.WalletMicroservice.Presentation.WebApi.Contracts;

public record LotInfoRequestData(
    Guid Id,
    string Title,
    string Description);
