using AlzaShop.Api.Controllers;
using AlzaShop.Core;
using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using AlzaShop.Core.Database.Entities;
using AlzaShop.Core.Models;
using AlzaShop.Core.Product;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Moq;
using System.Reflection.Metadata;
using ILogger = Serilog.ILogger;

namespace AlzaShop.Tests;

public class ProductControllerTests
{
    private readonly Mock<IMediator> mediatorMock;
    private readonly Mock<ILogger> loggerMock;
    private readonly ProductsController controller;
    private readonly Mock<IRepository<Product>> repositoryMock;


    public ProductControllerTests()
    {
        mediatorMock = new Mock<IMediator>();
        loggerMock = new Mock<ILogger>();
        repositoryMock = new Mock<IRepository<Product>>();

        controller = new ProductsController(mediatorMock.Object, loggerMock.Object);
    }


    [Fact]
    public async Task Index_ListOfProducts()
    {
        var expectedProducts = new List<Product>()
        {
            new Product { Id = 7, Name = "Mobile phone", Price = 25000, Description = "This is mobile phone description" },
            new Product { Id = 8, Name = "Laptop", Price = 65000, Description = "This is laptop description" },
            new Product { Id = 9, Name = "Washing maschine", Price = 15000 },
            new Product { Id = 10, Name = "Monitor", Price = 8000, Description = "This is monitor description" },
            new Product { Id = 11, Name = "Headphones", Price = 2000, Description = "This is headphones description" }
        };

        mediatorMock.Setup(m => m.Send(It.IsAny<ProductQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResponse<List<Product>>.Success(expectedProducts));

        var result = await controller.Index();

        result.Result.Should().BeEquivalentTo(expectedProducts);
        mediatorMock.Verify(x => x.Send(It.IsAny<ProductQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IndexV2_PaginatedListOfProducts()
    {
        var parameters = new PagingParameters { PageNumber = 1, PageSize = 10 };

        var expectedProducts = new List<Product>
        {
            new Product { Id = 7, Name = "Mobile phone", Price = 25000, Description = "This is mobile phone description" },
            new Product { Id = 8, Name = "Laptop", Price = 65000, Description = "This is laptop description" }
        };
        var expectedResult = new PagedResult<Product>(expectedProducts, 2, 1, 10);

        mediatorMock.Setup(m => m.Send(It.Is<ProductWithPaginationQuery>(q =>
                q.PageNumber == parameters.PageNumber 
                && q.PageSize == parameters.PageSize),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResponse<PagedResult<Product>>.Success(expectedResult));

        var result = await controller.IndexV2(parameters);

        result.Result.Should().BeEquivalentTo(expectedResult);
        result.Result.Items.Should().HaveCount(2);
        result.Result.PageSize.Should().Be(10);
        result.Result.PageNumber.Should().Be(1);
    }

    [Theory]
    [InlineData(7)]
    public async Task IndexV2_GetOneProduct(int id)
    {
        var expectedResult = new Product()
        { 
            Id = 7,
            Name = "Mobile phone",
            Price = 25000,
            Description = "This is mobile phone description" 
        };

        mediatorMock.Setup(m => m.Send(It.IsAny<ProductDetailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommandResponse<Product>.Success(expectedResult));

        var result = await controller.Detail(id);

        result.Result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData(9, "New description")]
    public async Task Update_ValidModel_ReturnsOkResult(int id, string newDescription)
    {
        var command = new ProductUpdateCommand(id)
        {
            Description = newDescription
        };

        var updatedProduct = new Product
        {
            Id = id,
            Description = "New description",
            Name = "Washing maschine",
            ImgUri = "https://shop.alza.cz/images/washing.png",
            Price = 15000
        };

        var commandResponse = CommandResponse<Product>.Success(updatedProduct);

        mediatorMock.Setup(m => m.Send(It.Is<ProductUpdateCommand>(c => c.Id == id && c.Description == command.Description),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResponse);

        var result = await controller.Update(id, command);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<CommandResponse<Product>>(okResult.Value);
        Assert.True(returnedResponse.IsValid);
        Assert.Equal(id, returnedResponse?.Result?.Id);
        Assert.Equal(updatedProduct.Description, returnedResponse?.Result?.Description);
    }

    [Fact]
    public async Task Update_InvalidModelState_ReturnsBadRequest()
    {
        var command = new ProductUpdateCommand(1);
        controller.ModelState.AddModelError("Description", "Required");

        var result = await controller.Update(1, command);

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task Update_ProductDoesNotExist_ReturnsNotFound()
    {
        var productId = 99;
        var command = new ProductUpdateCommand(productId)
        { 
            Description = "" 
        };

        var response = CommandResponse<Product>.Error(new CommandError(ErrorCodes.NotFoundException, "Product not found"));

        mediatorMock.Setup(m => m.Send(It.IsAny<ProductUpdateCommand>(), default)).ReturnsAsync(response);

        var result = await controller.Update(productId, command);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<CommandResponse<Product>>(okResult.Value);

        Assert.Null(returnedResponse.Result);
        Assert.Contains(ErrorCodes.NotFoundException, returnedResponse.Errors.Select(x => x.Code));
    }
}
