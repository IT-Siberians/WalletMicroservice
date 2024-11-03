namespace Auction.Common.Application.L2.Interfaces.Commands;

public record PageQuery(
    int ItemsCount,
    int Number)
{
    public static PageQuery? NewOrNull(int? pageItemsCount, int? pageNumber)
    {

        if (pageItemsCount is not null && pageNumber is not null && pageItemsCount.Value > 0 && pageNumber.Value > 0)
        {
            return new PageQuery(pageItemsCount.Value, pageNumber.Value);
        }

        return null;
    }
}
