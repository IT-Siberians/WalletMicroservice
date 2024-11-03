namespace Auction.Common.Application.L2.Interfaces.Answers;

public interface IBadBaseAnswer : IAnswer;

public interface IBadBaseAnswer<TResult> : IBadBaseAnswer, IAnswer<TResult>;
