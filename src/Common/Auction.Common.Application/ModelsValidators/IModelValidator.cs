namespace Auction.Common.Application.ModelsValidators;

public interface IModelValidator<TModel> where TModel : class
{
    string[]? GetErrors(TModel model);
}
