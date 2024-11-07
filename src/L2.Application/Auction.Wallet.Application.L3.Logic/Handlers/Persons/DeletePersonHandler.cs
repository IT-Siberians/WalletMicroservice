using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Common.Application.L3.Logic.Handlers;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using Auction.WalletMicroservice.Domain.Entities;

namespace Auction.Wallet.Application.L3.Logic.Handlers.Persons;

public class DeletePersonHandler(
    IOwnersRepository repository)
        : DeleteHandler<DeletePersonCommand, Owner, IOwnersRepository>(
            CommonNames.User,
            repository);
