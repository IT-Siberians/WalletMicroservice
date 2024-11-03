namespace Auction.Common.Application.L2.Interfaces.Commands;

public interface IPagedQuery
{
    PageQuery? Page { get; }
}
