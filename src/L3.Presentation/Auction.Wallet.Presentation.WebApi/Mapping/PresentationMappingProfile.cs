using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Traiding;
using Auction.Wallet.Presentation.WebApi.Contracts;
using AutoMapper;

namespace Auction.Wallet.Presentation.WebApi.Mapping;

public class PresentationMappingProfile : Profile
{
    public PresentationMappingProfile()
    {
        CreateMap<PutMoneyInWalletCommandHttp, PutMoneyInWalletCommand>();
        CreateMap<WithdrawMoneyFromWalletCommandHttp, WithdrawMoneyFromWalletCommand>();

        CreateMap<ReserveMoneyCommandHttp, ReserveMoneyCommand>();
        CreateMap<RealeaseMoneyCommandHttp, RealeaseMoneyCommand>();
        CreateMap<PayForLotCommandHttp, PayForLotCommand>();
        CreateMap<LotInfoModelHttp, LotInfoModel>();

        CreateMap<GetItemsPageByIdQuery, GetWalletTransactionsQuery>()
            .ConstructUsing(x => new GetWalletTransactionsQuery(x.Id));
    }
}

