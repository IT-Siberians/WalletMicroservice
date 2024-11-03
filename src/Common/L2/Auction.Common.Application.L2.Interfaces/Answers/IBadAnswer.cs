namespace Auction.Common.Application.L2.Interfaces.Answers;

public interface IBadAnswer : IBadBaseAnswer
{
    string? ErrorMessage { get; }

    ErrorCode ErrorCode { get; }
}

public interface IBadAnswer<TResult> : IBadAnswer, IBadBaseAnswer<TResult>, IAnswer<TResult>;
