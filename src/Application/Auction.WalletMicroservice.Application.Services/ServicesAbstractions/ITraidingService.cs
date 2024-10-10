using Auction.Common.Application.Responses;
using Auction.WalletMicroservice.Application.Models.Traiding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Application.Services.ServicesAbstractions;

public interface ITraidingService : IDisposable
{
    Task<BaseResponse> ReserveMoneyAsync(
        ReserveMoneyModel model,
        CancellationToken cancellationToken = default);

    Task<BaseResponse> RealeaseMoneyAsync(
        RealeaseMoneyModel model,
        CancellationToken cancellationToken = default);

    Task<BaseResponse> PayForLotAsync(
        PayForLotModel model,
        CancellationToken cancellationToken = default);
}
