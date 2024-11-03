using System.Collections.Generic;

namespace Auction.Common.Application.L2.Interfaces.Pages;

public record PageOf<TItem>(
    int ItemsCount,
    int PageItemsCount,
    int PagesCount,
    int PageNumber,
    IList<TItem> Items) : IPageOf<TItem>;
