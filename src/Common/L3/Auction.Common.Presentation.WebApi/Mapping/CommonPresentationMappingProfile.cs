using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Presentation.Contracts;
using AutoMapper;

namespace Auction.Common.Presentation.Mapping;

public class CommonPresentationMappingProfile : Profile
{
    public CommonPresentationMappingProfile()
    {
        CreateMap<CreatePersonCommandHttp, CreatePersonCommand>();
        CreateMap<IdModel, DeletePersonCommand>();
        CreateMap<IdModel, GetPersonQuery>();
        CreateMap<IdModel, IsPersonCommand>();
    }
}