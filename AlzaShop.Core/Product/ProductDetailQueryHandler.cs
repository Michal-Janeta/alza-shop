using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlzaShop.Core.Product;

public class ProductDetailQueryHandler : IRequestHandler<ProductDetailQuery, CommandResponse<Database.Entities.Product>>
{
    private readonly IRepository<Database.Entities.Product> repository;
    private readonly ILogger<ProductDetailQueryHandler> logger;


    public ProductDetailQueryHandler(IRepository<Database.Entities.Product> repository, ILogger<ProductDetailQueryHandler> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<CommandResponse<Database.Entities.Product>> Handle(ProductDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetByIdAsync(request.Id);

            if (result == null)
            {
                return CommandResponse<Database.Entities.Product>.Error(
                    new CommandError(ErrorCodes.NotFoundException, $"Product with ID {request.Id} not found"));
            }

            return CommandResponse<Database.Entities.Product>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while loading product detail for productId: {request.Id}");
            return CommandResponse<Database.Entities.Product>.Error(
                new CommandError(ErrorCodes.ExceptionOccured, "An error occurred while loading product detail for productId"));
        }
    }
}
