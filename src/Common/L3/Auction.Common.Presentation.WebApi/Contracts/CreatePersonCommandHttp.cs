using System;

namespace Auction.Common.Presentation.Contracts;

public record CreatePersonCommandHttp(
    Guid Id,
    string Username);
