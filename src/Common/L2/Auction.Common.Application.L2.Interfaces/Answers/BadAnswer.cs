namespace Auction.Common.Application.L2.Interfaces.Answers;

public class BadAnswer : IBadAnswer
{
    public string ErrorMessage { get; }

    public ErrorCode ErrorCode { get; }

    protected BadAnswer(string message, ErrorCode code)
    {
        ErrorMessage = message;
        ErrorCode = code;
    }

    public BadAnswer<TResult> ToBadAnswer<TResult>() => new(ErrorMessage, ErrorCode);

    public IAnswer<TResult> ToIAnswer<TResult>() => new BadAnswer<TResult>(ErrorMessage, ErrorCode);

    public static BadAnswer Error(string message) => new(message, ErrorCode.Error);

    public static BadAnswer Error(string pattern, params object?[] args) => new(string.Format(pattern, args), ErrorCode.Error);

    public static BadAnswer BadFieldValue(string message) => new(message, ErrorCode.BadFieldValue);

    public static BadAnswer BadFieldValue(string pattern, params object?[] args) => new(string.Format(pattern, args), ErrorCode.BadFieldValue);

    public static BadAnswer EntityNotFound(string message) => new(message, ErrorCode.EntityNotFound);

    public static BadAnswer EntityNotFound(string pattern, params object?[] args) => new(string.Format(pattern, args), ErrorCode.EntityNotFound);
}

public class BadAnswer<TResult>(string message, ErrorCode code)
    : BadAnswer(message, code),
    IBadAnswer<TResult>
{
    public static new BadAnswer<TResult> Error(string message) => new(message, ErrorCode.Error);

    public static new BadAnswer<TResult> Error(string pattern, params object?[] args) => new(string.Format(pattern, args), ErrorCode.Error);

    public static new BadAnswer<TResult> BadFieldValue(string message) => new(message, ErrorCode.BadFieldValue);

    public static new BadAnswer<TResult> BadFieldValue(string pattern, params object?[] args) => new(string.Format(pattern, args), ErrorCode.BadFieldValue);

    public static new BadAnswer<TResult> EntityNotFound(string message) => new(message, ErrorCode.EntityNotFound);

    public static new BadAnswer<TResult> EntityNotFound(string pattern, params object?[] args) => new(string.Format(pattern, args), ErrorCode.EntityNotFound);
}