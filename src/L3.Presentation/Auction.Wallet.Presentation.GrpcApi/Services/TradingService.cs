using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.Common.Domain.ValueObjects.String;
using Auction.Wallet.Application.L2.Interfaces.Commands.Trading;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace Auction.Wallet.Presentation.GrpcApi.Services;

public class TradingService(
    ICommandHandler<PayForLotCommand> payHandler,
    ICommandHandler<RealeaseMoneyCommand> realeaseHandler,
    ICommandHandler<ReserveMoneyCommand> reserveHandler)
        : Trading.TradingBase
{
    public override async Task<BaseResponseGrpc> PayForLot(PayForLotCommandGrpc request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.BuyerId, out _))
        {
            return GetErrorResponse($"�������� �������� BuyerId �� ��������������� ������� Guid: {request.BuyerId}");
        }
        if (!Guid.TryParse(request.SellerId, out _))
        {
            return GetErrorResponse($"�������� �������� SellerId �� ��������������� ������� Guid: {request.SellerId}");
        }
        if (!Guid.TryParse(request.LotId, out _))
        {
            return GetErrorResponse($"�������� �������� LotId �� ��������������� ������� Guid: {request.LotId}");
        }
        if (!Price.IsValid((decimal)request.HammerPrice))
        {
            return GetErrorResponse($"�������� ������������ HammerPrice: {request.HammerPrice}");
        }

        var query = new PayForLotCommand(
                        BuyerId: new Guid(request.BuyerId),
                        SellerId: new Guid(request.SellerId),
                        LotId: new Guid(request.LotId),
                        HammerPrice: (decimal)request.HammerPrice);

        var answer = await payHandler.HandleAsync(query, context.CancellationToken);

        return GetResponse(answer);
    }

    public override async Task<BaseResponseGrpc> RealeaseMoney(RealeaseMoneyCommandGrpc request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.BuyerId, out _))
        {
            return GetErrorResponse($"�������� �������� BuyerId �� ��������������� ������� Guid: {request.BuyerId}");
        }
        if (!Guid.TryParse(request.LotId, out _))
        {
            return GetErrorResponse($"�������� �������� LotId �� ��������������� ������� Guid: {request.LotId}");
        }
        if (!Price.IsValid((decimal)request.Price))
        {
            return GetErrorResponse($"�������� ������������ Price: {request.Price}");
        }

        var query = new RealeaseMoneyCommand(
                        BuyerId: new Guid(request.BuyerId),
                        LotId: new Guid(request.LotId),
                        Price: (decimal)request.Price);

        var answer = await realeaseHandler.HandleAsync(query, context.CancellationToken);

        return GetResponse(answer);
    }

    public override async Task<BaseResponseGrpc> ReserveMoney(ReserveMoneyCommandGrpc request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.BuyerId, out _))
        {
            return GetErrorResponse($"�������� �������� BuyerId �� ��������������� ������� Guid: {request.BuyerId}");
        }
        if (!Guid.TryParse(request.Lot.Id, out _))
        {
            return GetErrorResponse($"�������� �������� Lot.Id �� ��������������� ������� Guid: {request.Lot.Id}");
        }
        if (!Title.IsValid(request.Lot.Title))
        {
            return GetErrorResponse($"�������� ������������ �������� Lot.Title: {request.Lot.Title}");
        }
        if (!Text.IsValid(request.Lot.Description))
        {
            return GetErrorResponse($"�������� ������������ �������� Lot.Description: {request.Lot.Description}");
        }
        if (!Price.IsValid((decimal)request.Price))
        {
            return GetErrorResponse($"�������� ������������ �������� Price: {request.Price}");
        }

        var query = new ReserveMoneyCommand(
                        BuyerId: new Guid(request.BuyerId),
                        Lot: new LotInfoModel(
                            Id: new Guid(request.Lot.Id),
                            Title: request.Lot.Title,
                            Description: request.Lot.Description),
                        Price: (decimal)request.Price);

        var answer = await reserveHandler.HandleAsync(query, context.CancellationToken);

        return GetResponse(answer);
    }

    private static BaseResponseGrpc GetResponse(IAnswer answer)
    {
        if (answer is IOkAnswer okAnswer)
        {
            return GetOkResponse(okAnswer.Message ?? "Ok");
        }
        else if (answer is IBadAnswer badAnswer)
        {
            return GetErrorResponse(badAnswer.ErrorMessage ?? "Error");
        }
        else
        {
            return GetErrorResponse(CommonMessages.UnknownError);
        }
    }

    private static BaseResponseGrpc GetErrorResponse(string errorMessage)
    {
        return new BaseResponseGrpc
        {
            IsError = true,
            Message = errorMessage
        };
    }

    private static BaseResponseGrpc GetOkResponse(string okMessage)
    {
        return new BaseResponseGrpc
        {
            IsError = false,
            Message = okMessage
        };
    }
}
