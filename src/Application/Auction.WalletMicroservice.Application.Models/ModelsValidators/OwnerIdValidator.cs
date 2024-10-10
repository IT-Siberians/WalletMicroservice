using Auction.Common.Application.ModelsValidators;
using Auction.WalletMicroservice.Application.Models.Owner;
using System;

namespace Auction.WalletMicroservice.Application.Models.ModelsValidators;

public class OwnerIdValidator : IModelValidator<OwnerIdModel>
{
    public string[]? GetErrors(OwnerIdModel model)
    {
        if (model.OwnerId == Guid.Empty)
        {
            return ["Id владельца не может быть равен Guid.Empty"];
        }

        return null;
    }
}
