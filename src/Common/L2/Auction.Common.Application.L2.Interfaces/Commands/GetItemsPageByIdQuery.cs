using Auction.Common.Application.L1.Models;
using System;

namespace Auction.Common.Application.L2.Interfaces.Commands;

public record GetItemsPageByIdQuery(
    Guid Id,
    PageQuery? Page,
    FilterQuery? Filter)
        : IModel<Guid>, IPagedQuery, IFilteredQuery;
