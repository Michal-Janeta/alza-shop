using AlzaShop.Core.Commands;
using MediatR;

namespace AlzaShop.Core.Product;

public class ProductDetailQuery : IRequest<CommandResponse<Database.Entities.Product>>
{
    public int Id { get; set; }

    public ProductDetailQuery(int id)
    {
        Id = id;
    }
}
