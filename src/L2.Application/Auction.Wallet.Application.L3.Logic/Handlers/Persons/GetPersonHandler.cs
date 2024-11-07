using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Common.Application.L3.Logic.Handlers;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using Auction.WalletMicroservice.Domain.Entities;
using AutoMapper;

namespace Auction.Wallet.Application.L3.Logic.Handlers.Persons;

public class GetPersonHandler(
    IOwnersRepository repository,
    IMapper mapper)
        : GetByIdHandler<GetPersonQuery, Owner, PersonInfoModel, IOwnersRepository>(
            CommonNames.User,
            repository,
            toModel: e => mapper.Map<PersonInfoModel>(e),
            includeProperties: null,
            useTracking: false);
