using Auction.Common.Application.Models.Common;
using Auction.WalletMicroservice.Domain.Entities;
using AutoMapper;

namespace Auction.WalletMicroservice.Application.Services.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<PersonInfoModel, Owner>();
        CreateMap<Owner, PersonInfoModel>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value));

        CreateMap<LotInfoModel, Lot>();
        CreateMap<Lot, LotInfoModel>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value));
    }
}
