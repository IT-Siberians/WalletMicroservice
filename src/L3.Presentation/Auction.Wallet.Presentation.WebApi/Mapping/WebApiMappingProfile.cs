using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Wallet.Application.L2.Interfaces.Commands.Owners;
using Auction.Wallet.Application.L2.Interfaces.Commands.Trading;
using Auction.Wallet.Presentation.WebApi.Contracts.Owner;
using Auction.Wallet.Presentation.WebApi.Contracts.Trading;
using AutoMapper;

namespace Auction.Wallet.Presentation.WebApi.Mapping;

public class WebApiMappingProfile : Profile
{
    public WebApiMappingProfile()
    {
        CreateMap<PutMoneyInWalletCommandWeb, PutMoneyInWalletCommand>();
        CreateMap<WithdrawMoneyFromWalletCommandWeb, WithdrawMoneyFromWalletCommand>();

        CreateMap<ReserveMoneyCommandWeb, ReserveMoneyCommand>();
        CreateMap<RealeaseMoneyCommandWeb, RealeaseMoneyCommand>();
        CreateMap<PayForLotCommandWeb, PayForLotCommand>();
        CreateMap<LotInfoModelWeb, LotInfoModel>();

        CreateMap<GetItemsPageByIdQuery, GetWalletTransactionsQuery>()
            .ConstructUsing(x => new GetWalletTransactionsQuery(x.Id));
    }
}
