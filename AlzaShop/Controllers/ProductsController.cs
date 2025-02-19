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
    public Task<CommandResponse<List<Core.Database.Entities.Product>>> Index() 
        => ExecuteCommand(new ProductQuery());
    

    [HttpGet("detail/{id}")]
    public async Task<CommandResponse<Core.Database.Entities.Product>> Detail(int id)
    {
        var result = await ExecuteCommand(new ProductDetailQuery(id));

        return result;
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse<int>>> Update(int id, ProductUpdateCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        command.Id = id;

        var result = await ExecuteCommand(command);

        return Ok(result);
    }

}
