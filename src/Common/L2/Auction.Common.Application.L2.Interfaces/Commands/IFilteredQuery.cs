namespace Auction.Common.Application.L2.Interfaces.Commands;

public interface IFilteredQuery
{
    FilterQuery? Filter { get; }
}
