using jAPS.API.Db;
using jAPS.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http.Headers;

namespace jAPS.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainController : ControllerBase
    {
        JapsDbContext context;

        public MainController(JapsDbContext _context)
        {
            context = _context;
        }

        [HttpPost("AddToBasket")]
        public async Task<ActionResult<Basket>> AddToBasket([FromBody] Basket request)
        {
            if (request.BasketId is null) request.BasketId = Guid.NewGuid();

            List<Product> items = context.Products.Where(p => request.Items.Select(i => i.ProductId).Contains(p.ProductId)).ToList();

            request.Items.ForEach(item =>
            {
                item.Product = items.FirstOrDefault(id => id.ProductId == item.ProductId);
            });

            var totalPrice = request.Items.Sum(x => x.Product.Price * x.Quantity);

            var trans = new Transaction
            {
                BasketId = (Guid)request.BasketId,
                CreatedAt = DateTime.Now,
                Order = new Order
                {
                    TotalPrice = totalPrice,
                    OrderItems = items.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        PriceAtSale = item.Price,
                        Quantity = request.Items.First(p => p.ProductId == item.ProductId).Quantity,
                    }).ToList()
                },
                PaymentMethod = Models.Enums.PaymentMethod.Card,
                PaymentProvider = Models.Enums.PaymentProvider.GoPay,
                TransactionState = Models.Enums.TransactionState.New,
            };

            context.Transactions.Add(trans);

            await context.SaveChangesAsync();

            return Ok(new Basket
            {
                BasketId = (Guid)request.BasketId,
                Items = request.Items,
                TotalPrice = totalPrice
            });
        }

        [HttpPost("RemoveFromBasket")]
        public async Task<ActionResult<Basket>> RemoveFromBasket([FromQuery] Guid basketId, [FromBody] ProductDto request)
        {
            List<OrderItem> items = new();
            try
            {
                items = context.OrdersItems
                               .Include(x => x.Product)
                               .Include(x => x.Order) //-------------- ??? what decimal shit is this
                               .Where(x => x.Order.Transaction.BasketId == basketId)
                               .ToList();
            }
            catch (Exception ex)
            {


            }




            var itemToRemove = items.First(x => x.Product.ProductId == request.ProductId);
            int itemCount = itemToRemove.Quantity - request.Quantity;

            if (itemCount <= 0)
            {
                context.OrdersItems.RemoveRange(itemToRemove);
            }
            else { context.OrdersItems.Update(itemToRemove); }


            await context.SaveChangesAsync();


            items = context.OrdersItems
                               .Include(x => x.Product)
                               .Where(x => x.Order.Transaction.BasketId == basketId)
                               .ToList();

            var totalPrice = items.Sum(x => x.Product.Price * x.Quantity);
            var products = context.OrdersItems.Where(x => x.Order.Transaction.BasketId == basketId).ToList();

            //items[0].Order.TotalPrice = totalPrice;
            context.Orders.Update(items[0].Order);

            var basketItems = new List<ProductDto>();
            products.ForEach(x =>
            {
                basketItems.Add(new ProductDto { ProductId = x.ProductId, Product = x.Product, Quantity = x.Quantity });
            });

            return Ok(new Basket
            {
                BasketId = basketId,
                Items = basketItems,
                TotalPrice = totalPrice
            });
        }

        [HttpGet("GetBasket")]
        public async Task<ActionResult<Basket>> GetBasket([FromRoute] Guid basketId)
        {
            return Ok(new Product { Description = "asdads" });
        }

        [HttpPost("FinnishOrder")]
        public async Task<ActionResult<OrderDetail>> GetBasket([FromRoute] Guid basketId, [FromBody] Customer request)
        {
            return Ok(new OrderDetail { });
        }
    }
}
