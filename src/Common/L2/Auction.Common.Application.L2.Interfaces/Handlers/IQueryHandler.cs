using Auction.Common.Application.L2.Interfaces.Answers;

namespace Auction.Common.Application.L2.Interfaces.Handlers;

public interface IQueryHandler<TQuery, TResponse>
    : IHandler<TQuery, IAnswer<TResponse>>
        where TQuery : class;
