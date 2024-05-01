﻿using AutoMapper;
using jAPS.API.Db;
using jAPS.API.Models;
using MediatR;
using NuGet.Protocol.Core.Types;

namespace jAPS.API.Queries
{
    public class GetBasketQuery : IRequest<Basket>
    {
        public Guid basketId { get; set; }
    }

    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Basket>
    {
        MainRepository _repository;
        IMapper _mapper;
        public GetBasketQueryHandler(MainRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Basket> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetBasketItems(request.basketId);

            var totalPrice = items.Sum(x => x.Product.Price * x.Quantity);

            return new Basket
            {
                BasketId = request.basketId,
                Items = _mapper.Map<List<ProductDto>>(items),
                TotalPrice = totalPrice
            };
            
        }
    }
}
