using Auction.Common.Application.Responses;
using Auction.WalletMicroservice.Application.Models.Traiding;
using Auction.WalletMicroservice.Application.Services.ServicesAbstractions;
using Auction.WalletMicroservice.Presentation.WebApi.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Presentation.WebApi.Controllers;

[ApiController]
[Route("/api/v3/[controller]/[action]")]
public class TraidingController(
    ITraidingService traidingService,
    IMapper mapper)
        : ControllerBase
{
    private readonly ITraidingService _traidingService = traidingService ?? throw new ArgumentNullException(nameof(traidingService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> ReserveMoney(
        [FromBody] ReserveMoneyRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _traidingService
            .ReserveMoneyAsync(
                _mapper.Map<ReserveMoneyModel>(request),
                cancellationToken);

        return this.GetActionResult(response);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> RealeaseMoney(
        [FromBody] RealeaseMoneyRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _traidingService
            .RealeaseMoneyAsync(
                _mapper.Map<RealeaseMoneyModel>(request),
                cancellationToken);

        return this.GetActionResult(response);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> PayForLot(
        [FromBody] PayForLotRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _traidingService
            .PayForLotAsync(
                _mapper.Map<PayForLotModel>(request),
                cancellationToken);

        return this.GetActionResult(response);
    }
}
