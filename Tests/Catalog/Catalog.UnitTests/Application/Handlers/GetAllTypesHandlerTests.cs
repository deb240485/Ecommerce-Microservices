using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers;

public class GetAllTypesHandlerTests
    {
        private readonly Mock<ITypesRepository> _mockRepository;
        private readonly GetAllTypesHandler _handler;

        public GetAllTypesHandlerTests()
        {
            _mockRepository = new Mock<ITypesRepository>();
            _handler = new GetAllTypesHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WhenTypesExist_ReturnsTypesList()
        {
            // Arrange
            var query = new GetAllTypesQuery();

            var types = new List<ProductType>
            {
                new ProductType { Id = "1", Name = "Electronics" },
                new ProductType { Id = "2", Name = "Clothing" }
            };

            _mockRepository
                .Setup(x => x.GetAllTypes())
                .ReturnsAsync(types);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockRepository.Verify(x => x.GetAllTypes(), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoTypes_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetAllTypesQuery();

            _mockRepository
                .Setup(x => x.GetAllTypes())
                .ReturnsAsync(new List<ProductType>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }