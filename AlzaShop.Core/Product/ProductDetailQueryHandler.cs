using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using MediatR;

namespace AlzaShop.Core.Product;

public class ProductDetailQueryHandler : IRequestHandler<ProductDetailQuery, CommandResponse<Database.Entities.Product>>
{
    private readonly IRepository<Database.Entities.Product> repository;

    public ProductDetailQueryHandler(IRepository<Database.Entities.Product> repository)
    {
        this.repository = repository;
    }

    public async Task<CommandResponse<Database.Entities.Product>> Handle(ProductDetailQuery request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse<Database.Entities.Product>();

        try
        {
            var result = await repository.GetByIdAsync(request.Id);

            return new CommandResponse<Database.Entities.Product>()
            {
                Result = result
            };
        }
        catch (Exception ex)
        {
            response.Errors.Add(new CommandError(ErrorCodes.ExceptionOccured, ex.Message));
        }

        return response;
    }
}
