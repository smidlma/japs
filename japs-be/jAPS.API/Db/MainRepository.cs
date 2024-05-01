using jAPS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace jAPS.API.Db;

public class MainRepository
{
    JapsDbContext _context;
    public MainRepository(JapsDbContext context)
    {
        _context = context;
    }

    public Task<List<Product>> GetAllProductsAsync() =>
        _context.Products.ToListAsync();

    public Task<List<OrderItem>> GetBasketItems(Guid basketId) =>
        _context.OrdersItems
                .Include(x => x.Product)
                .Where(x => x.Order.Transaction.BasketId == basketId)
                .ToListAsync();



}
