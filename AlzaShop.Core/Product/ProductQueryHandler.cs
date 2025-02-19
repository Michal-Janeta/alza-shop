using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlzaShop.Core.Product;

public class ProductQueryHandler : IRequestHandler<ProductQuery, CommandResponse<List<Database.Entities.Product>>>
{
    private readonly IRepository<Database.Entities.Product> repository;
    private readonly ILogger<ProductQueryHandler> logger;

    public ProductQueryHandler(IRepository<Database.Entities.Product> repository, ILogger<ProductQueryHandler> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<CommandResponse<List<Database.Entities.Product>>> Handle(ProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.GetAllAsync();

            return CommandResponse<List<Database.Entities.Product>>.Success(result.ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while loading products");
            return CommandResponse<List<Database.Entities.Product>>.Error(
                new CommandError(ErrorCodes.ExceptionOccured, "An error occurred while loading products"));
        }
    }
}
