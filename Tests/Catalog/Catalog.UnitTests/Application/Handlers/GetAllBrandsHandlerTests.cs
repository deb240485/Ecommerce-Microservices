using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers;

public class GetAllBrandsHandlerTests
{
    private readonly Mock<IBrandRepository> _mockRepository;
    private readonly GetAllBrandsHandler _handler;

    public GetAllBrandsHandlerTests()
    {
        _mockRepository = new Mock<IBrandRepository>();
        _handler = new GetAllBrandsHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenBrandsExist_ReturnsBrandList()
    {
        // Arrange
        var query = new GetAllBrandsQuery();

        var brands = new List<ProductBrand>
            {
                new ProductBrand { Id = "1", Name = "Apple" },
                new ProductBrand { Id = "2", Name = "Samsung" }
            };

        _mockRepository
            .Setup(x => x.GetAllBrands())
            .ReturnsAsync(brands);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(x => x.GetAllBrands(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoBrands_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllBrandsQuery();

        _mockRepository
            .Setup(x => x.GetAllBrands())
            .ReturnsAsync(new List<ProductBrand>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}