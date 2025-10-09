using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Xunit;

namespace Catalog.UnitTests.Application.Queries;

public class GetAllTypesQueryTests
{
    [Fact]
    public void GetAllTypesQuery_CanBeInstantiated()
    {
        // Act
        var query = new GetAllTypesQuery();

        // Assert
        Assert.NotNull(query);
    }

    [Fact]
    public void GetAllTypesQuery_ImplementsIRequest()
    {
        // Act
        var query = new GetAllTypesQuery();

        // Assert
        Assert.IsAssignableFrom<IRequest<IList<TypesResponse>>>(query);
    }

    [Fact]
    public void GetAllTypesQuery_MultipleInstances_AreIndependent()
    {
        // Act
        var query1 = new GetAllTypesQuery();
        var query2 = new GetAllTypesQuery();

        // Assert
        Assert.NotSame(query1, query2);
    }
}
