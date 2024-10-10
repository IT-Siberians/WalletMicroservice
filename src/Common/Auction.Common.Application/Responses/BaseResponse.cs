namespace Auction.Common.Application.Responses;

public class BaseResponse
{
    public bool IsSuccess { get; set; }

    //public bool IsSuccessMessage => !string.IsNullOrEmpty(SuccessMessage);

    public string? SuccessMessage { get; set; }

    public string[]? ErrorMessages { get; set; }

    public static BaseResponse Success()
    {
        return new BaseResponse
        {
            IsSuccess = true
        };
    }

    public static BaseResponse Success(string message)
    {
        return new BaseResponse
        {
            IsSuccess = true,
            SuccessMessage = message
        };
    }

    public static BaseResponse Error(string message)
    {
        return new BaseResponse
        {
            ErrorMessages = [message]
        };
    }

    public static BaseResponse Error(string[] messages)
    {
        return new BaseResponse
        {
            ErrorMessages = messages
        };
    }
}
