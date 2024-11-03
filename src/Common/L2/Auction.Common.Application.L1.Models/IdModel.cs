using System;

namespace Auction.Common.Application.L1.Models;

public record IdModel(
        Guid Id)
            : IModel<Guid>;
