using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Presentation.Contracts;
using Auction.Common.Presentation.Controllers;
using AutoMapper;

namespace Auction.Wallet.Presentation.WebApi.Controllers;

public class PersonController(IMapper mapper)
        : CreateDeleteApiController<CreatePersonCommandWeb, CreatePersonCommand, DeletePersonCommand>(mapper);
