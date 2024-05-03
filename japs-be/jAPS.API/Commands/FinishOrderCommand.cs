using AutoMapper;
using jAPS.API.Db;
using jAPS.API.Models;
using MediatR;

namespace jAPS.API.Commands
{
    public class FinishOrderCommand : IRequest<OrderDetail>
    {
        public Guid BasketId {  get; set; }
        public Customer Customer { get; set; }
    }

    public class FinnishOrderCommandHandler : IRequestHandler<FinishOrderCommand, OrderDetail> 
    {
        private IMapper _mapper;
        private MainRepository _repository;
        public FinnishOrderCommandHandler(IMapper mapper, MainRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDetail> Handle(FinishOrderCommand request, CancellationToken cancellationToken) 
        {
            var trans = await _repository.GetTransaction(request.BasketId);
            await _repository.AppendCustomerToTransaction(trans, request.Customer);
            trans.Customer = request.Customer;
            
            var ord = _mapper.Map<OrderDetail>(trans);

            return ord;
        }
    }
}
