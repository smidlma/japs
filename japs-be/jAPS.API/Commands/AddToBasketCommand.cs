using AutoMapper;
using jAPS.API.Db;
using jAPS.API.Models;
using MediatR;

namespace jAPS.API.Commands
{
    public class AddToBasketCommand : IRequest<Basket>
    {
        public Basket Basket { get; set; }
    }

    public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, Basket>
    {
        private IMapper _mapper;
        private MainRepository _repository;
        public AddToBasketCommandHandler(IMapper mapper, MainRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Basket> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetBasketProducts(request.Basket.Items);

            request.Basket.Items.ForEach(item =>
            {
                item.Product = items.FirstOrDefault(id => id.ProductId == item.ProductId);
            });

            var totalPrice = request.Basket.Items.Sum(x => x.Product.Price * x.Quantity);

            Transaction trans = request.Basket.BasketId is null ?
                new Transaction
                {
                    BasketId = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Order = new Order
                    {
                        TotalPrice = totalPrice,
                        OrderItems = request.Basket.Items.Select(item => new OrderItem
                        {
                            ProductId = item.ProductId,
                            PriceAtSale = item.Product?.Price ?? 0,
                            Quantity = item.Quantity,
                        }).ToList()
                    },
                    PaymentMethod = Models.Enums.PaymentMethod.Card,
                    PaymentProvider = Models.Enums.PaymentProvider.GoPay,
                    TransactionState = Models.Enums.TransactionState.New
                }
                : await _repository.GetTransaction((Guid)request.Basket.BasketId);            

            if (request.Basket.BasketId is null)
                await _repository.AddTransaction(trans);
            else
            {
                items.ForEach(item =>
                {
                    var quantityToAdd = request.Basket.Items.First(p => p.ProductId == item.ProductId).Quantity;
                    var existingOrderItem = trans.Order.OrderItems.FirstOrDefault(orderItem => orderItem.ProductId == item.ProductId);

                    if (existingOrderItem != null)
                    {
                        existingOrderItem.Quantity += quantityToAdd;
                    }
                    else
                    {
                        trans.Order.OrderItems.Add(new OrderItem
                        {
                            ProductId = item.ProductId,
                            PriceAtSale = item.Price,
                            Quantity = quantityToAdd
                        });
                    }
                });

                trans.Order.TotalPrice += totalPrice;
                await _repository.UpdateTransaction(trans);
            }

            return _mapper.Map<Basket>(trans);
        }
    }
}
