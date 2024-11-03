using Auction.Common.Application.L2.Interfaces.Pages;

namespace Auction.Common.Application.L2.Interfaces.Handlers;

public interface IQueryPageHandler<TQuery, TResponse>
    : IQueryHandler<TQuery, IPageOf<TResponse>>
        where TQuery : class;
