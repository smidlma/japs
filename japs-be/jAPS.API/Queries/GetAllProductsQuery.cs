using jAPS.API.Db;
using jAPS.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace jAPS.API.Queries
{

    public class GetAllProductsQuery : IRequest<ICollection<Product>> {}

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ICollection<Product>>
    {
        private MainRepository _repository;
        public GetAllProductsQueryHandler(MainRepository repository)
        {
            _repository = repository;
        }
        public async Task<ICollection<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllProductsAsync();
        }

    }
}
