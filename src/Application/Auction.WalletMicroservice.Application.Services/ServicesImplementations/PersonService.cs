using Auction.Common.Application.Models.Common;
using Auction.Common.Application.ModelsValidators;
using Auction.Common.Application.ServicesImplementations;
using Auction.WalletMicroservice.Domain.Entities;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using AutoMapper;

namespace Auction.WalletMicroservice.Application.Services.ServicesImplementations;

public class PersonService(
    IOwnersRepository repository,
    IModelValidator<PersonIdModel> personIdValidator,
    IModelValidator<PersonInfoModel> personInfoValidator,
    IMapper mapper)
        : PersonService<Owner, IOwnersRepository>(
            repository,
            personIdValidator,
            personInfoValidator,
            mapper);
