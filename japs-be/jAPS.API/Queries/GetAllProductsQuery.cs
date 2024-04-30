using jAPS.API.Db;
using jAPS.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace jAPS.API.Queries
{

    public class GetAllProductsQuery : IRequest<ICollection<Product>>
    {

    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ICollection<Product>>
    {
        JapsDbContext _context;
        public GetAllProductsQueryHandler(JapsDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync();
        }

    }
}
