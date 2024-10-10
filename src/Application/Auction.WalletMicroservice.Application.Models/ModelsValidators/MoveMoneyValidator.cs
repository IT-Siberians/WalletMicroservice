using Auction.Common.Application.ModelsValidators;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Application.Models.Owner;
using System;
using System.Collections.Generic;

namespace Auction.WalletMicroservice.Application.Models.ModelsValidators;

public class MoveMoneyValidator : IModelValidator<MoveMoneyModel>
{
    public string[]? GetErrors(MoveMoneyModel model)
    {
        var errors = new List<string>();

        if (model.OwnerId == Guid.Empty)
        {
            errors.Add("Id пользователя не может быть равен Guid.Empty");
        }

        if (!Money.IsValid(model.Money))
        {
            errors.Add("Неправильное количество денег");
        }

        if (errors.Count == 0)
        {
            return null;
        }

        return errors.ToArray();
    }
}
