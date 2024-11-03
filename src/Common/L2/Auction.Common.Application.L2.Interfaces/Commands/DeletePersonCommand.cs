using Auction.Common.Application.L1.Models;
using System;

namespace Auction.Common.Application.L2.Interfaces.Commands;

public record DeletePersonCommand(Guid Id)
        : IModel<Guid>;
