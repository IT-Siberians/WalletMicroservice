using Auction.Common.Application.Models.Common;
using Auction.Common.Application.Responses;
using Auction.Common.Application.ServicesAbstractions;
using Auction.Common.Presentation.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Auction.WalletMicroservice.Presentation.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class PersonController(
    IPersonService personService,
    IMapper mapper)
        : ControllerBase
{
    private readonly IPersonService _personService = personService ?? throw new ArgumentNullException(nameof(personService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> CreatePerson(
        [FromBody] CreatePersonRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _personService
            .CreatePersonAsync(
                _mapper.Map<PersonInfoModel>(request),
                cancellationToken);

        return this.GetActionResult(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse>> UpdatePerson(
        Guid id,
        [FromBody] UpdatePersonRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _personService
            .UpdatePersonAsync(
                new PersonInfoModel(id, request.Username),
                cancellationToken);

        return this.GetActionResult(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<BaseResponse>> DeletePerson(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await _personService
            .DeletePersonAsync(
                new PersonIdModel(id),
                cancellationToken);

        return this.GetActionResult(response);
    }
}
