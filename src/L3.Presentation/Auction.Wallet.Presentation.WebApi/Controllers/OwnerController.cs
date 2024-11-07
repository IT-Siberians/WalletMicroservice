using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Pages;
using Auction.Common.Presentation.Controllers;
using Auction.Wallet.Application.L1.Models.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Presentation.WebApi.Contracts.Owner;
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
public class OwnerController(
    IMapper mapper)
        : ControllerBase
{
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkAnswer))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BadAnswer))]
    public Task<ActionResult<IAnswer>> PutMoneyInWallet(
        [FromBody] PutMoneyInWalletCommandWeb command,
        [FromServices] ICommandHandler<PutMoneyInWalletCommand> handler,
        [FromServices] IValidator<PutMoneyInWalletCommand> validator,
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
    public Task<ActionResult<IAnswer>> WithdrawMoneyFromWallet(
        [FromBody] WithdrawMoneyFromWalletCommandWeb command,
        [FromServices] ICommandHandler<WithdrawMoneyFromWalletCommand> handler,
        [FromServices] IValidator<WithdrawMoneyFromWalletCommand> validator,
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

    [HttpGet("{ownerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkAnswer<BalanceModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BadAnswer))]
    public async Task<ActionResult<IAnswer<BalanceModel>>> GetWalletBalance(
        [FromRoute] Guid ownerId,
        [FromServices] IQueryHandler<GetWalletBalanceQuery, BalanceModel> handler,
        [FromServices] IValidator<GetWalletBalanceQuery> validator,
        CancellationToken cancellationToken)
    {
        var query = new GetWalletBalanceQuery(ownerId);

        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return this.GetBadRequest<BalanceModel>(validationResult);
        }

        var answer = await handler.HandleAsync(query, cancellationToken);

        return this.GetActionResult(answer);
    }

    [HttpGet("{ownerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkAnswer<IPageOf<TransactionModel>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadValues))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BadAnswer))]
    public Task<ActionResult<IAnswer<IPageOf<TransactionModel>>>> GetWalletTransactions(
        [FromRoute] Guid ownerId,
        [FromServices] ICommandHandler<IsPersonCommand> isHandler,
        [FromServices] IQueryPageHandler<GetWalletTransactionsQuery, TransactionModel> getHandler,
        [FromServices] IValidator<GetWalletTransactionsQuery> validator,
        CancellationToken cancellationToken)
    {
        var query = ControllersHelper.GetPageByIdQuery(
                        ownerId,
                        null,
                        null,
                        null,
                        null);

        return this.GetPageByIdAsync(
                        query,
                        isHandler,
                        getHandler,
                        validator,
                        _mapper,
                        cancellationToken);
    }
}
