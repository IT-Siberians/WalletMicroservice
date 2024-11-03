namespace Auction.Common.Application.L2.Interfaces.Answers;

public interface IOkBaseAnswer : IAnswer;

public interface IOkAnswer : IOkBaseAnswer
{
    string? Message { get; }
}

public interface IOkAnswer<TResult> : IOkBaseAnswer, IAnswer<TResult>
{
    TResult Result { get; }
}
