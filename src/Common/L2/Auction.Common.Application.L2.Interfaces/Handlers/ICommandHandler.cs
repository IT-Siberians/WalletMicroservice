using Auction.Common.Application.L2.Interfaces.Answers;

namespace Auction.Common.Application.L2.Interfaces.Handlers;

public interface ICommandHandler<TCommand>
    : IHandler<TCommand, IAnswer>
        where TCommand : class;
