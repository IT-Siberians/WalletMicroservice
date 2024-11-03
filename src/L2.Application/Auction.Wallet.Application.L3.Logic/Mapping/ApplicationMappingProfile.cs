using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.WalletMicroservice.Domain.Entities;
using AutoMapper;

namespace Auction.Wallet.Application.L3.Logic.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<CreatePersonCommand, Owner>();

        CreateMap<PersonInfoModel, Owner>();
        CreateMap<Owner, PersonInfoModel>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value));

        CreateMap<LotInfoModel, Lot>();
        CreateMap<Lot, LotInfoModel>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value));
    }
}
