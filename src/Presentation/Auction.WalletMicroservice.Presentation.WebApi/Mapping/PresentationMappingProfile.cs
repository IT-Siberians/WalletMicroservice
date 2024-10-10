using Auction.Common.Application.Models.Common;
using Auction.Common.Presentation.Contracts;
using Auction.WalletMicroservice.Application.Models.Owner;
using Auction.WalletMicroservice.Application.Models.Traiding;
using Auction.WalletMicroservice.Presentation.WebApi.Contracts;
using AutoMapper;

namespace Auction.WalletMicroservice.Presentation.WebApi.Mapping;

public class PresentationMappingProfile : Profile
{
    public PresentationMappingProfile()
    {
        CreateMap<CreatePersonRequest, PersonInfoModel>();

        CreateMap<MoveMoneyRequest, MoveMoneyModel>();

        CreateMap<ReserveMoneyRequest, ReserveMoneyModel>();
        CreateMap<RealeaseMoneyRequest, RealeaseMoneyModel>();
        CreateMap<PayForLotRequest, PayForLotModel>();
        CreateMap<LotInfoRequestData, LotInfoModel>();
    }
}

