using Auction.Common.Application.Models.Base;
using System;

namespace Auction.Common.Application.Models.Common;

public record PersonInfoModel(
        Guid Id,
        string Username
    )
        : IModel<Guid>;
