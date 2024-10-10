namespace Auction.Common.Application.Responses;

public class Response<T> : BaseResponse
    where T : class
{
    public T? Value { get; set; }

    public static Response<T> Success(T value)
    {
        return new Response<T>
        {
            IsSuccess = true,
            Value = value
        };
    }

    public static Response<T> Success(T value, string message)
    {
        return new Response<T>
        {
            IsSuccess = true,
            Value = value,
            SuccessMessage = message
        };
    }

    public static new Response<T> Error(string message)
    {
        return new Response<T>
        {
            ErrorMessages = [message]
        };
    }

    public static new Response<T> Error(string[] messages)
    {
        return new Response<T>
        {
            ErrorMessages = messages
        };
    }
}
