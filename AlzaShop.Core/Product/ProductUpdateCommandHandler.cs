using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using MediatR;

namespace AlzaShop.Core.Product;

public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, CommandResponse<Database.Entities.Product>>
{
    private readonly IRepository<Database.Entities.Product> repository;

    public ProductUpdateCommandHandler(IRepository<Database.Entities.Product> repository)
    {
        this.repository = repository;
    }

    public async Task<CommandResponse<Database.Entities.Product>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse<Database.Entities.Product>();

        try
        {
            var product = await repository.GetByIdAsync(request.Id);

            if (product == null)
            {
                response.Errors.Add(new CommandError(ErrorCodes.NotFoundException, "Product doesn't exist"));
                return response;
            }

            product.Description = request.Description;

            await repository.SaveAsync(product);

            return new CommandResponse<Database.Entities.Product>()
            {
                Result = product
            };
        }
        catch (Exception ex)
        {
            response.Errors.Add(new CommandError(ErrorCodes.ExceptionOccured, ex.Message));
        }

        return response;
    }
}
