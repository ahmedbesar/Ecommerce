using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public sealed class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IEnumerable<ProductTypeResponseDto>>
    {
        private readonly ITypeRepository _repository;
        private readonly TypeMapper _mapper;
        public GetAllTypesQueryHandler(ITypeRepository repository, TypeMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<ProductTypeResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _repository.GetAllTypesAsync(cancellationToken);
            var response = _mapper.ToResponseListDto(types);
            return response;
        }
    }
}
