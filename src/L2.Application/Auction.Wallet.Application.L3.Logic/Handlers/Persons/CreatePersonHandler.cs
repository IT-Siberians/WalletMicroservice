using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Common.Application.L3.Logic.Handlers;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using Auction.WalletMicroservice.Domain.Entities;
using AutoMapper;

namespace Auction.Wallet.Application.L3.Logic.Handlers.Persons;

public class CreatePersonHandler(
    IOwnersRepository repository,
    IMapper mapper)
        : CreateHandler<CreatePersonCommand, Owner, IOwnersRepository>(
            CommonNames.User,
            repository,
            mapper);
