using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlzaShop.Core.Product;

public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, CommandResponse<Database.Entities.Product>>
{
    private readonly IRepository<Database.Entities.Product> repository;
    private readonly ILogger<ProductUpdateCommandHandler> logger;

    public ProductUpdateCommandHandler(IRepository<Database.Entities.Product> repository, ILogger<ProductUpdateCommandHandler> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<CommandResponse<Database.Entities.Product>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await repository.GetByIdAsync(request.Id);

            if (product == null)
            {

                return CommandResponse<Database.Entities.Product>.Error(
                    new CommandError(ErrorCodes.NotFoundException, $"Product with ID {request.Id} not found"));
            }

            product.Description = request.Description;

            await repository.SaveAsync(product);

            return CommandResponse<Database.Entities.Product>.Success(product);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while updating the product description for productId: {request.Id}");
            return CommandResponse<Database.Entities.Product>.Error(
                new CommandError(ErrorCodes.ExceptionOccured, "An error occurred while updating the product description"));
        }
    }
}
