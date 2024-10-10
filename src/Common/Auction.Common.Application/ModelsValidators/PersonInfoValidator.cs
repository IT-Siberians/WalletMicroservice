using Auction.Common.Application.Models.Common;
using Auction.Common.Domain.ValueObjects.String;
using System;
using System.Collections.Generic;

namespace Auction.Common.Application.ModelsValidators;

public class PersonInfoValidator : IModelValidator<PersonInfoModel>
{
    public string[]? GetErrors(PersonInfoModel model)
    {
        var errors = new List<string>();

        if (model.Id == Guid.Empty)
        {
            errors.Add("Id пользователя не может быть равен Guid.Empty");
        }

        if (!Username.IsValid(model.Username))
        {
            errors.Add("Неправильное значение Username");
        }

        if (errors.Count == 0)
        {
            return null;
        }

        return errors.ToArray();
    }
}
