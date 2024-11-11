using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Handlers;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Logging;
using Otus.QueueDto.User;
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
            logger.LogWarning($"Validate Failed: Username = {command.Username}, Id = {command.Id}");
            return;
        }

        var answer = await handler.HandleAsync(command, new CancellationToken());
        if (answer is BadAnswer badAnswer)
        {
            logger.LogError($"Error message: {badAnswer.ErrorMessage}");
        }
        else
        {
            logger.LogInformation($"Created user: Username = {command.Username}, Id = {command.Id}");
        }
    }
}
