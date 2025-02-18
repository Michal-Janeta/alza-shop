using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using MediatR;

namespace AlzaShop.Core.Product;

public class ProductQueryHandler : IRequestHandler<ProductQuery, CommandResponse<List<Database.Entities.Product>>>
{
    private readonly IRepository<Database.Entities.Product> repository;

    public ProductQueryHandler(IRepository<Database.Entities.Product> repository)
    {
        this.repository = repository;
    }

    public async Task<CommandResponse<List<Database.Entities.Product>>> Handle(ProductQuery request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse<List<Database.Entities.Product>>();

        try
        {
            var result = await repository.GetAllAsync();
            var products = new List<Database.Entities.Product>();

            return new CommandResponse<List<Database.Entities.Product>>()
            {
                Result = result.ToList(),
            };
        }
        catch (Exception ex)
        {
            response.Errors.Add(new CommandError(ErrorCodes.ExceptionOccured, ex.Message));
        }

        return response;
    }
}
