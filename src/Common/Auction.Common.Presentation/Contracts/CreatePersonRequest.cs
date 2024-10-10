using System;

namespace Auction.Common.Presentation.Contracts;

public record CreatePersonRequest(
    Guid Id,
    string Username);
