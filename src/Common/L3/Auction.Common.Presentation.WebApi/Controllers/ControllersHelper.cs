using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Pages;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Presentation.Controllers;

public static class ControllersHelper
{
    public static ActionResult<TAnswer> GetActionResult<TAnswer>(
        this ControllerBase controller,
        TAnswer answer,
        bool isCreated = false)
            where TAnswer : IAnswer
    {
        return answer switch
        {
            IOkBaseAnswer => isCreated
                            ? controller.Created("", answer)
                            : controller.Ok(answer),
            IBadAnswer badAnswer => badAnswer?.ErrorCode == ErrorCode.EntityNotFound
                            ? controller.NotFound(answer)
                            : controller.BadRequest(answer),
            _ => controller.BadRequest(answer)
        };
    }

    public static async Task<ActionResult<IAnswer>> GetCommandActionResultAsync<TCommand, TCommandHttp>(
        this ControllerBase controller,
        TCommandHttp commandHttp,
        ICommandHandler<TCommand> handler,
        IValidator<TCommand> validator,
        IMapper mapper,
        bool isCreated,
        CancellationToken cancellationToken)
            where TCommand : class
            where TCommandHttp : class
    {
        var command = mapper.Map<TCommand>(commandHttp);

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return controller.GetBadRequest(validationResult);
        }

        var answer = await handler.HandleAsync(command, cancellationToken);

        return controller.GetActionResult(answer, isCreated);
    }

    public static ActionResult<IAnswer> GetBadRequest(
        this ControllerBase controller,
        ValidationResult validationResult)
    {
        return controller.BadRequest(new BadValues(validationResult.ToDictionary()));
    }

    public static ActionResult<IAnswer<TResult>> GetBadRequest<TResult>(
        this ControllerBase controller,
        ValidationResult validationResult)
    {
        return controller.BadRequest(new BadValues<TResult>(validationResult.ToDictionary()));
    }

    public static async Task<ActionResult<IAnswer<IPageOf<TItemModel>>>> GetPageByIdAsync<TQuery, TItemModel, TIsCommand>(
        this ControllerBase controller,
        GetItemsPageByIdQuery query,
        ICommandHandler<TIsCommand> isHandler,
        IQueryPageHandler<TQuery, TItemModel> getHandler,
        IValidator<TQuery> validator,
        IMapper mapper,
        CancellationToken cancellationToken)
        where TQuery : class
            where TIsCommand : class, IModel<Guid>
    {
        var specificQuery = mapper.Map<TQuery>(query);

        var validationResult = validator.Validate(specificQuery);
        if (!validationResult.IsValid)
        {
            return controller.GetBadRequest<IPageOf<TItemModel>>(validationResult);
        }

        var idModel = new IdModel(query.Id);
        var isEntityCommand = mapper.Map<TIsCommand>(idModel);
        var isEntityAnswer = await isHandler.HandleAsync(isEntityCommand, cancellationToken);
        if (isEntityAnswer is BadAnswer badAnswer)
        {
            return controller.GetActionResult(badAnswer.ToIAnswer<IPageOf<TItemModel>>());
        }

        var answer = await getHandler.HandleAsync(specificQuery, cancellationToken);

        return controller.GetActionResult(answer);
    }

    public static GetItemsPageByIdQuery GetPageByIdQuery(
        Guid id,
        int? pageItemsCount,
        int? pageNumber,
        string? with,
        string? without)
    {
        var page = PageQuery.NewOrNull(pageItemsCount, pageNumber);
        var filter = FilterQuery.NewOrNull(with, without);
        var query = new GetItemsPageByIdQuery(id, page, filter);

        return query;
    }
}
