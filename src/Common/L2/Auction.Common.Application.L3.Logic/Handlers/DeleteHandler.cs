using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Repositories.Base;
using Auction.Common.Application.L2.Interfaces.Repositories.Partial;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Common.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Application.L3.Logic.Handlers;

public class DeleteHandler<TCommand, TEntity, TEntityRepository>(
    string entityName,
    TEntityRepository repository)
        : ICommandHandler<TCommand>,
        IDisposable
            where TCommand : class, IModel<Guid>
            where TEntity : class, IEntity<Guid>, IDeletableSoftly
            where TEntityRepository : class, IBaseRepository<TEntity, Guid>, IDeletableRepository<TEntity, Guid>
{
    private readonly TEntityRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly string _entityName = entityName ?? throw new ArgumentNullException(nameof(entityName));

    private bool _isDisposed;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _repository.Dispose();

            _isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public async Task<IAnswer> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _repository.GetByIdAsync(command.Id, cancellationToken: cancellationToken);
        if (existingEntity is null)
        {
            return BadAnswer.EntityNotFound(CommonMessages.DoesntExistWithId, _entityName, command.Id);
        }

        _repository.Delete(existingEntity);
        await _repository.SaveChangesAsync(cancellationToken);

        return new OkAnswer(CommonMessages.DeletedWithId, _entityName, command.Id);
    }
}
