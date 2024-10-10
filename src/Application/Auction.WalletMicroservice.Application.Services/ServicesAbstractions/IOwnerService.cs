using Auction.Common.Application.Responses;
using Auction.WalletMicroservice.Application.Models.Owner;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.WalletMicroservice.Application.Services.ServicesAbstractions;

public interface IOwnerService : IDisposable
{
    Task<BaseResponse> PutMoneyInWalletAsync(
        MoveMoneyModel model,
        CancellationToken cancellationToken = default);

    Task<BaseResponse> WithdrawMoneyFromWalletAsync(
        MoveMoneyModel model,
        CancellationToken cancellationToken = default);

    Task<Response<BalanceModel>> GetWalletBalanceAsync(
        OwnerIdModel model,
        CancellationToken cancellationToken = default);

    Task<Response<IList<TransactionModel>>> GetWalletTransactionsAsync(
        OwnerIdModel model,
        CancellationToken cancellationToken = default);
}
