using Auction.Common.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WalletMicroservice.Presentation.WebApi.Controllers;

public static class Helper
{
    public static ActionResult<T> GetActionResult<T>(
        this ControllerBase controller,
        T response)
            where T : BaseResponse
    {
        if (response.IsSuccess)
        {
            return controller.Ok(response);
        }

        return controller.BadRequest(response);
    }
}
