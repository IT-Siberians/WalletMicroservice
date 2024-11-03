using System;

namespace Auction.Common.Application.L1.Models;

public record PersonInfoModel(
        Guid Id,
        string Username)
            : IModel<Guid>;
