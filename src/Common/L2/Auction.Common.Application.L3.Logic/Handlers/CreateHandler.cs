using Auction.Common.Application.L1.Models;
using Auction.Common.Application.L2.Interfaces.Answers;
using Auction.Common.Application.L2.Interfaces.Handlers;
using Auction.Common.Application.L2.Interfaces.Repositories.Base;
using Auction.Common.Application.L2.Interfaces.Strings;
using Auction.Common.Domain.Entities;
using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Application.L3.Logic.Handlers;

public class CreateHandler<TCommand, TEntity, TEntityRepository>(
    string entityName,
    TEntityRepository repository,
    IMapper mapper)
        : ICommandHandler<TCommand>,
        IDisposable
            where TCommand : class, IModel<Guid>
            where TEntity : class, IEntity<Guid>
            where TEntityRepository : class, IBaseRepository<TEntity, Guid>
{
    private readonly TEntityRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        if (existingEntity is not null)
        {
            return BadAnswer.Error(CommonMessages.AlreadyExistsWithId, _entityName, command.Id);
        }

        var newEntity = _mapper.Map<TEntity>(command);

        await _repository.AddAsync(newEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new OkAnswer(CommonMessages.CreatedWithId, _entityName, command.Id);
    }
}
