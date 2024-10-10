using Auction.Common.Application.Models.Common;
using Auction.Common.Application.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Application.ServicesAbstractions;

public interface IPersonService : IDisposable
{
    Task<BaseResponse> CreatePersonAsync(
        PersonInfoModel model,
        CancellationToken cancellationToken = default);

    Task<BaseResponse> UpdatePersonAsync(
        PersonInfoModel model,
        CancellationToken cancellationToken = default);

    Task<BaseResponse> DeletePersonAsync(
        PersonIdModel model,
        CancellationToken cancellationToken = default);
}
