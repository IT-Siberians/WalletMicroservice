using Auction.Common.Application.L1.Models;
using System;

namespace Auction.Common.Application.L2.Interfaces.Commands;

public record IsPersonCommand(Guid Id)
        : IModel<Guid>;
