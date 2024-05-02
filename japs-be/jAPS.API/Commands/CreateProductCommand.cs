using jAPS.API.Db;
using jAPS.API.Models;
using MediatR;
using NuGet.Protocol.Core.Types;

namespace jAPS.API.Commands;

public class CreateProductCommand : IRequest<Product>
{
    public Product Product { get; set; }

}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
{
    MainRepository _repository;
    public CreateProductCommandHandler(MainRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken) =>
         await _repository.AddProductAsync(request.Product);

}