using System;

namespace Auction.WalletMicroservice.Presentation.WebApi.Contracts;

public record LotInfoDto(
    Guid Id,
    string Title,
    string Description);
