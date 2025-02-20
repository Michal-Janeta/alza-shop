using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using AlzaShop.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AlzaShop.Core.Product;

public class ProductWithPaginationQueryHandler : IRequestHandler<ProductWithPaginationQuery, CommandResponse<PagedResult<Database.Entities.Product>>>
{
    private readonly IRepository<Database.Entities.Product> repository;
    private readonly ILogger<ProductWithPaginationQueryHandler> logger;


    public ProductWithPaginationQueryHandler(IRepository<Database.Entities.Product> repository, ILogger<ProductWithPaginationQueryHandler> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<CommandResponse<PagedResult<Database.Entities.Product>>> Handle(ProductWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var totalCount = await repository.GetQueryable().CountAsync(cancellationToken);

            var items = await repository.GetQueryable()
                                   .Skip((request.PageNumber - 1) * request.PageSize)
                                   .Take(request.PageSize)
                                   .ToListAsync(cancellationToken);

            var result = new PagedResult<Database.Entities.Product>(items, totalCount, request.PageNumber, request.PageSize);

            return CommandResponse<PagedResult<Database.Entities.Product>>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while loading pagination product list");
            return CommandResponse<PagedResult<Database.Entities.Product>>.Error(
                new CommandError(ErrorCodes.ExceptionOccured, "An error occurred while loading pagination product list"));
        }
    }
}
