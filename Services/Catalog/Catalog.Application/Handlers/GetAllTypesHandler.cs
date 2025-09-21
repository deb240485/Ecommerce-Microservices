using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
    {
        private readonly ITypesRepository _typesRepository;
        public GetAllTypesHandler(ITypesRepository typesRepository)
        {
             _typesRepository = typesRepository;   
        }
        public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var typeList = await _typesRepository.GetAllTypes();
            var typesResponseList = ProductMapper.Mapper.Map<IList<ProductType>, IList<TypesResponse>>(typeList.ToList());
            return typesResponseList;
        }
    }
}
