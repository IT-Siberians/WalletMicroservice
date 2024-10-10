using Auction.Common.Application.Models.Common;
using System;

namespace Auction.WalletMicroservice.Application.Models.Traiding;

public record ReserveMoneyModel(
    Guid BuyerId,
    decimal Price,
    LotInfoModel Lot);
