using AlzaShop.Core.Commands;
using MediatR;

namespace AlzaShop.Core.Product;

public class ProductQuery : IRequest<CommandResponse<List<Database.Entities.Product>>>
{
}
