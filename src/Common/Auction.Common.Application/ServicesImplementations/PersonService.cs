using Auction.Common.Application.Models.Common;
using Auction.Common.Application.ModelsValidators;
using Auction.Common.Application.Responses;
using Auction.Common.Application.ServicesAbstractions;
using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Base;
using Auction.Common.Domain.ValueObjects.String;
using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Application.ServicesImplementations;

public class PersonService<TPerson, TPersonRepository>(
    TPersonRepository repository,
    IModelValidator<PersonIdModel> personIdValidator,
    IModelValidator<PersonInfoModel> personInfoValidator,
    IMapper mapper)
        : IPersonService
            where TPerson : AbstractPerson<Guid>
            where TPersonRepository : IBaseRepositoryWithUpdateAndDelete<TPerson, Guid>
{
    private readonly TPersonRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    private readonly IModelValidator<PersonIdModel> _personIdValidator = personIdValidator ?? throw new ArgumentNullException(nameof(personIdValidator));
    private readonly IModelValidator<PersonInfoModel> _personInfoValidator = personInfoValidator ?? throw new ArgumentNullException(nameof(personInfoValidator));

    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private bool _isDisposed;

    public async Task<BaseResponse> CreatePersonAsync(
        PersonInfoModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _personInfoValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var existingEntity = await _repository.GetByIdAsync(model.Id, cancellationToken: cancellationToken);
        if (existingEntity is not null)
        {
            return BaseResponse.Error($"Уже существует пользователь с Id = {model.Id}");
        }

        var newEntity = _mapper.Map<TPerson>(model);

        await _repository.AddAsync(newEntity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success($"Пользователь '{newEntity.Username}' создан");
    }

    public async Task<BaseResponse> UpdatePersonAsync(
        PersonInfoModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _personInfoValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var existingEntity = await _repository.GetByIdAsync(model.Id, cancellationToken: cancellationToken);
        if (existingEntity is null)
        {
            return BaseResponse.Error($"Не существует пользователь с Id = {model.Id}");
        }

        var username = new Username(model.Username);

        existingEntity.ChangeUsername(username);

        _repository.Update(existingEntity);
        await _repository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success("Параметры пользователя обновлены");
    }

    public async Task<BaseResponse> DeletePersonAsync(
        PersonIdModel model,
        CancellationToken cancellationToken = default)
    {
        var errors = _personIdValidator.GetErrors(model);
        if (errors is not null)
        {
            return BaseResponse.Error(errors);
        }

        var existingEntity = await _repository.GetByIdAsync(model.Id, cancellationToken: cancellationToken);
        if (existingEntity is null)
        {
            return BaseResponse.Error($"Не существует пользователь с Id = {model.Id}");
        }

        _repository.Delete(existingEntity);
        await _repository.SaveChangesAsync(cancellationToken);

        return BaseResponse.Success($"Пользователь '{existingEntity.Username}' удалён");
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _repository.Dispose();

            _isDisposed = true;
        }
    }
}
