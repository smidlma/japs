using jAPS.API.Db;
using jAPS.API.Models;
using jAPS.API.Queries;
using MediatR;

namespace jAPS.API.Queries;

public class GetProductQuery : IRequest<Product>
{
    public int ProductId { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    MainRepository _repository;
    public GetProductQueryHandler(MainRepository repository)
    {
        _repository = repository;
    }
    public Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken) =>
         _repository.GetProductAsync(request.ProductId);
}

