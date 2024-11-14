using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Handlers;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using Otus.QueueDto.User;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Wallet.Presentation.MassTransit.Persons;

public class CreateUserConsumer(
    IValidator<CreatePersonCommand> validator,
    ICommandHandler<CreatePersonCommand> handler,
    ILogger<CreateUserConsumer> logger)
        : IConsumer<CreateUserEvent>
{
    public async Task Consume(ConsumeContext<CreateUserEvent> context)
    {
        var command = new CreatePersonCommand(context.Message.Id, context.Message.Username);

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            var messages = $"{Environment.NewLine}{string.Join(Environment.NewLine, validationResult.Errors.Select(e => $"- {e.PropertyName}: {e.ErrorMessage}"))}";
            logger.LogError("Validation messages: {Messages}", messages);
            return;
        }

        var answer = await handler.HandleAsync(command, new CancellationToken());
        if (answer is BadAnswer badAnswer)
        {
            logger.LogError("Error message: {ErrorMessage}", badAnswer.ErrorMessage);
        }
        else
        {
            logger.LogInformation("Created user: Username = {Username}, Id = {Id}", command.Username, command.Id);
        }
    }
}
