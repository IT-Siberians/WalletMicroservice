using Auction.Common.Application.L1.Models;
using System;

namespace Auction.Common.Application.L2.Interfaces.Commands;

public record GetPersonQuery(Guid Id)
        : IModel<Guid>;
