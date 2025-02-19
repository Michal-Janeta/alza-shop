using AlzaShop.Api.Controllers;
using AlzaShop.Core;
using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using AlzaShop.Core.Database.Entities;
using AlzaShop.Core.Models;
using AlzaShop.Core.Product;
using FluentAssertions;
using MediatR;
using Moq;
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
            .ReturnsAsync(new CommandResponse<List<Product>>(expectedProducts));

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
            .ReturnsAsync(new CommandResponse<PagedResult<Product>>(expectedResult));

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
            .ReturnsAsync(new CommandResponse<Product>(expectedResult));

        var result = await controller.Detail(id);

        result.Result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData(9)]
    public async Task Handle_WhenProductExists_UpdatesDescription(int id)
    {
        var newDescription = "Updated description";
        var existingProduct = new Product { Id = id, Description = "Old description" };

        repositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(existingProduct);

        var command = new ProductUpdateCommand(id);

        var result = await controller.Update(id, command);
        var data = result.;
        result.Result. .Should().Be(newDescription);

        _repositoryMock.Verify(r => r.SaveAsync(
            It.Is<Product>(p => p.Id == id && p.Description == newDescription),
            _cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductNotFound_ReturnsError()
    {
        // Arrange
        var productId = 1;
        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, _cancellationToken))
            .ReturnsAsync((Product)null);

        var command = new UpdateProductDescriptionCommand
        {
            Id = productId,
            Description = "New description"
        };

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be(ErrorCodes.NotFoundException);

        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Product>(), _cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExceptionOccurs_ReturnsError()
    {
        // Arrange
        var productId = 1;
        _repositoryMock
            .Setup(r => r.GetByIdAsync(productId, _cancellationToken))
            .ThrowsAsync(new Exception("Test exception"));

        var command = new UpdateProductDescriptionCommand
        {
            Id = productId,
            Description = "New description"
        };

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be(ErrorCodes.ExceptionOccured);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }
}
