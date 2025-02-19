using AlzaShop.Core.Commands;
using AlzaShop.Core.Models;
using MediatR;

namespace AlzaShop.Core.Product;

public class ProductWithPaginationQuery : IRequest<CommandResponse<PagedResult<Database.Entities.Product>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public ProductWithPaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
