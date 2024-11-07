using System;

namespace Auction.Common.Presentation.Contracts;

public record CreatePersonCommandWeb(
    Guid Id,
    string Username);
