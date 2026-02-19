using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IEnumerable<ProductTypeResponse>>
    {
        private readonly ITypeRepository _repository;

        public GetAllTypesQueryHandler(ITypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductTypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _repository.GetAllTypes();
            var mapper = new TypeMapper();
            var response = mapper.ToResponseList(types);
            return response;
        }
    }
}
