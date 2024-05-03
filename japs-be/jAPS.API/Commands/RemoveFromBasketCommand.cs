using AutoMapper;
using jAPS.API.Db;
using jAPS.API.Models;
using MediatR;

namespace jAPS.API.Commands;

public class RemoveFromBasketCommand : IRequest<Basket>
{
    public Guid BasketId { get; set; }
    public ProductDto Product { get; set; }
}

public class RemoveFromBasketCommandHandler : IRequestHandler<RemoveFromBasketCommand, Basket>
{
    MainRepository _repository;
    IMapper _mapper;
    public RemoveFromBasketCommandHandler(MainRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Basket> Handle(RemoveFromBasketCommand request, CancellationToken cancellationToken)
    {
        List<OrderItem> items = await _repository.GetBasketItemsAsync(request.BasketId);

        var itemToRemove = items.First(x => x.Product.ProductId == request.Product.ProductId);
        int itemCount = itemToRemove.Quantity - request.Product.Quantity;

        if (itemCount <= 0)
        {
            await _repository.RemoveOrderItemAsync(itemToRemove);
            items.Remove(itemToRemove);
        }
        else
        {
            itemToRemove.Quantity = itemToRemove.Quantity - itemCount;
            await _repository.UpdateOrderItemAsync(itemToRemove);
        }

        var totalPrice = items.Sum(x => x.Product.Price * x.Quantity);

        if (items.Any())
        {
            items.First().Order.TotalPrice = totalPrice;
            await _repository.UpdateOrderAsync(items.First().Order);
        }

        return (new Basket
        {
            BasketId = request.BasketId,
            Items = _mapper.Map<List<ProductDto>>(items),
            TotalPrice = totalPrice
        });
    }
}
