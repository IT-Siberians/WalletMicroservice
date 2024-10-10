using Auction.Common.Application.Models.Base;
using System;

namespace Auction.Common.Application.Models.Common;

public record PersonIdModel(Guid Id)
        : IModel<Guid>;
