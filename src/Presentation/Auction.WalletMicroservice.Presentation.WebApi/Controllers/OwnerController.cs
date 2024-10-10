using Auction.Common.Application.Responses;
using Auction.WalletMicroservice.Application.Models.Owner;
using Auction.WalletMicroservice.Application.Services.ServicesAbstractions;
using Auction.WalletMicroservice.Presentation.WebApi.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Presentation.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class OwnerController(
    IOwnerService ownerService,
    IMapper mapper)
        : ControllerBase
{
    private readonly IOwnerService _ownerService = ownerService ?? throw new ArgumentNullException(nameof(ownerService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> PutMoney(
        [FromBody] MoveMoneyRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _ownerService
            .PutMoneyInWalletAsync(
                _mapper.Map<MoveMoneyModel>(request),
                cancellationToken);

        return this.GetActionResult(response);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> WithdrawMoney(
        [FromBody] MoveMoneyRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _ownerService
            .WithdrawMoneyFromWalletAsync(
                _mapper.Map<MoveMoneyModel>(request),
                cancellationToken);

        return this.GetActionResult(response);
    }

    [HttpGet("{ownerId:guid}")]
    public async Task<ActionResult<Response<BalanceModel>>> GetBalance(
        Guid ownerId,
        CancellationToken cancellationToken)
    {
        var response = await _ownerService
            .GetWalletBalanceAsync(
                new OwnerIdModel(ownerId),
                cancellationToken);

        return this.GetActionResult(response);
    }

    [HttpGet("{ownerId:guid}")]
    public async Task<ActionResult<Response<IList<TransactionModel>>>> GetTransactions(
        Guid ownerId,
        CancellationToken cancellationToken)
    {
        var response = await _ownerService
            .GetWalletTransactionsAsync(
                new OwnerIdModel(ownerId),
                cancellationToken);

        return this.GetActionResult(response);
    }
}
