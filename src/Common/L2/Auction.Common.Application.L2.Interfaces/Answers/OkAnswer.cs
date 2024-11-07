namespace Auction.Common.Application.L2.Interfaces.Answers;

public class OkAnswer(string message) : IOkAnswer
{
    public string Message { get; } = message;

    public OkAnswer(string pattern, params object?[] args) : this(string.Format(pattern, args)) { }
}

public class OkAnswer<TResult>(TResult result) : IOkAnswer<TResult>
{
    public TResult Result { get; } = result;
}
