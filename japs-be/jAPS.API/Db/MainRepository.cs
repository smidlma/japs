using Humanizer;
using jAPS.API.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace jAPS.API.Db;

public class MainRepository
{
    JapsDbContext _context;
    public MainRepository(JapsDbContext context)
    {
        _context = context;
    }

    public async Task RemoveOrderItemAsync(OrderItem item) { 
        _context.OrdersItems.Remove(item);
        await _context.SaveChangesAsync();        
    }

    public async Task UpdateOrderItemAsync(OrderItem item)
    {
        _context.OrdersItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public Task<Product> GetProductAsync(int productId) =>
        _context.Products.Where(p => p.ProductId == productId).FirstAsync();

    public Task<List<Product>> GetAllProductsAsync() =>
        _context.Products.ToListAsync();

    public Task<List<OrderItem>> GetBasketItemsAsync(Guid basketId) =>
        _context.OrdersItems
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Where(x => x.Order.Transaction.BasketId == basketId)
                .ToListAsync();

    public async Task<Product> AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

}
