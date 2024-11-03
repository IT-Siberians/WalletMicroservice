using System;

namespace Auction.Common.Application.L1.Models;

public record LotInfoModel(
        Guid Id,
        string Title,
        string Description)
            : IModel<Guid>;
