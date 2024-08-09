using System;

namespace Auction.Common.Domain.Exceptions;

public class DomainException(string message) : Exception(message);
