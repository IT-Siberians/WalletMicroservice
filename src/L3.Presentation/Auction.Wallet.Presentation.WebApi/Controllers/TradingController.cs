using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Presentation.Controllers;
using Auction.Wallet.Application.L2.Interfaces.Commands.Trading;
using Auction.Wallet.Presentation.WebApi.Contracts.Trading;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Wallet.Presentation.WebApi.Controllers;

[Route("/api/v1/[controller]/[action]")]
[ApiController]
public class TradingController(
    IMapper mapper)
        : ControllerBase
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkAnswer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BadAnswer))]
    public Task<ActionResult<IAnswer>> PayForLot(
        [FromBody] PayForLotCommandWeb command,
        [FromServices] ICommandHandler<PayForLotCommand> handler,
        [FromServices] IValidator<PayForLotCommand> validator,
        CancellationToken cancellationToken)
    {
        return this.GetCommandActionResultAsync(
                    command,
                    handler,
                    validator,
                    _mapper,
                    isCreated: false,
                    cancellationToken);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkAnswer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BadAnswer))]
    public Task<ActionResult<IAnswer>> RealeaseMoney(
        [FromBody] RealeaseMoneyCommandWeb command,
        [FromServices] ICommandHandler<RealeaseMoneyCommand> handler,
        [FromServices] IValidator<RealeaseMoneyCommand> validator,
        CancellationToken cancellationToken)
    {
        return this.GetCommandActionResultAsync(
                    command,
                    handler,
                    validator,
                    _mapper,
                    isCreated: false,
                    cancellationToken);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkAnswer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BadAnswer))]
    public Task<ActionResult<IAnswer>> ReserveMoney(
        [FromBody] ReserveMoneyCommandWeb command,
        [FromServices] ICommandHandler<ReserveMoneyCommand> handler,
        [FromServices] IValidator<ReserveMoneyCommand> validator,
        CancellationToken cancellationToken)
    {
        return this.GetCommandActionResultAsync(
                    command,
                    handler,
                    validator,
                    _mapper,
                    isCreated: false,
                    cancellationToken);
    }
}
