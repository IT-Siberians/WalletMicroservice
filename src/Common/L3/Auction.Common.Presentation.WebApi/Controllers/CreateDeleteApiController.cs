using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Presentation.Controllers;

[Route("/api/v1/[controller]")]
[ApiController]
public class CreateDeleteApiController<TCreateCommandHttp, TCreateCommand, TDeleteCommand>(
    IMapper mapper)
        : ControllerBase
            where TCreateCommandHttp : class
            where TCreateCommand : class
            where TDeleteCommand : class
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OkAnswer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    public async Task<ActionResult<IAnswer>> Create(
        [FromBody] TCreateCommandHttp commandHttp,
        [FromServices] IValidator<TCreateCommand> validator,
        [FromServices] ICommandHandler<TCreateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<TCreateCommand>(commandHttp);

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return this.GetBadRequest(validationResult);
        }

        var answer = await handler.HandleAsync(command, cancellationToken);

        return this.GetActionResult(answer, true);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkAnswer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BadAnswer))]
    public async Task<ActionResult<IAnswer>> Delete(
        Guid id,
        [FromServices] IValidator<TDeleteCommand> validator,
        [FromServices] ICommandHandler<TDeleteCommand> handler,
        CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<TDeleteCommand>(new IdModel(id));

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return this.GetBadRequest(validationResult);
        }

        var answer = await handler.HandleAsync(command, cancellationToken);

        return this.GetActionResult(answer);
    }
}
