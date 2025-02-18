using AlzaShop.Core.Commands;
using AlzaShop.Core.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace AlzaShop.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    public ProductsController(IMediator mediator, ILogger logger) : base(mediator, logger)
    {
    }

    [HttpPost]
    public async Task<CommandResponse<List<Core.Database.Entities.Product>>> Index()
    {
        var result = await ExecuteCommand(new ProductQuery());

        return result;
    }
}
