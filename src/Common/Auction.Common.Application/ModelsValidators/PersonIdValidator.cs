using Auction.Common.Application.Models.Common;
using System;

namespace Auction.Common.Application.ModelsValidators;

public class PersonIdValidator : IModelValidator<PersonIdModel>
{
    public string[]? GetErrors(PersonIdModel model)
    {
        if (model.Id == Guid.Empty)
        {
            return ["Id пользователя не может быть равен Guid.Empty"];
        }

        return null;
    }
}
