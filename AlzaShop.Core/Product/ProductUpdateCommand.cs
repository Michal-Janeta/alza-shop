using AlzaShop.Core.Commands;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace AlzaShop.Core.Product;

public class ProductUpdateCommand : IRequest<CommandResponse<Database.Entities.Product>>
{
    public int Id { get; set; }

    public string Description { get; set; }

    public ProductUpdateCommand(int id)
    {
        Id = id;
    }

}
