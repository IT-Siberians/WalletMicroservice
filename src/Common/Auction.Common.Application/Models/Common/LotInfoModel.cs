using Auction.Common.Application.Models.Base;
using System;

namespace Auction.Common.Application.Models.Common;

public record LotInfoModel(
        Guid Id,
        string Title,
        string Description
    )
        : IModel<Guid>;
