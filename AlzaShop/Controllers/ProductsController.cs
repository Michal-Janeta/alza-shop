using AlzaShop.Core.Commands;
using AlzaShop.Core.Database.Entities;
using AlzaShop.Core.Models;
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

    [HttpGet]
    public Task<CommandResponse<List<Product>>> Index() 
        => ExecuteCommand(new ProductQuery());

    [HttpGet("v2")]
    public Task<CommandResponse<PagedResult<Product>>> IndexV2([FromQuery] PagingParameters parameters)
        => ExecuteCommand(new ProductWithPaginationQuery(parameters.PageNumber, parameters.PageSize));


    [HttpGet("detail/{id}")]
    public Task<CommandResponse<Product>> Detail(int id)
        => ExecuteCommand(new ProductDetailQuery(id));

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse<Product>>> Update(int id, ProductUpdateCommand command)
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
