using AutoMapper;
using jAPS.API.Db;
using jAPS.API.Models;
using jAPS.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http.Headers;

namespace jAPS.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainController : ApiControllerBase
    {
        JapsDbContext context;
        IMapper mapper;

        public MainController(JapsDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        [HttpPost("AddToBasket")]
        public async Task<ActionResult<Basket>> AddToBasket([FromBody] Basket request)
        {
            List<Product> items = context.Products.Where(p => request.Items.Select(i => i.ProductId).Contains(p.ProductId)).ToList();

            //fill request with product items
            request.Items.ForEach(item =>
            {
                item.Product = items.FirstOrDefault(id => id.ProductId == item.ProductId);
            });

            var totalPrice = request.Items.Sum(x => x.Product.Price * x.Quantity);

            Transaction trans = request.BasketId is null ?
                new Transaction
                {
                    BasketId = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    Order = new Order
                    {
                        TotalPrice = totalPrice,
                        OrderItems = request.Items.Select(item => new OrderItem
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
                : context.Transactions
                          .Include(o => o.Order.OrderItems)
                          .First(x => x.BasketId == request.BasketId);

            items.ForEach(item =>
            {
                var quantityToAdd = request.Items.First(p => p.ProductId == item.ProductId).Quantity;
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


            if (request.BasketId is null) 
                context.Transactions.Add(trans);            
            else 
                context.Transactions.Update(trans);            

            await context.SaveChangesAsync();

            return Ok(mapper.Map<Basket>(trans));
        }

        [HttpPost("RemoveFromBasket/{basketId}")]
        public async Task<ActionResult<Basket>> RemoveFromBasket([FromRoute] Guid basketId, [FromBody] ProductDto request)
        {
            List<OrderItem> items = context.OrdersItems
                                           .Include(x => x.Product)
                                           .Include(x => x.Order)
                                           .Where(x => x.Order.Transaction.BasketId == basketId)
                                           .ToList();

            var itemToRemove = items.First(x => x.Product.ProductId == request.ProductId);
            int itemCount = itemToRemove.Quantity - request.Quantity;

            if (itemCount <= 0)
            {
                context.OrdersItems.RemoveRange(itemToRemove);
                items.Remove(itemToRemove);
            }
            else
            {
                context.OrdersItems.Update(itemToRemove);
                items.First().Quantity = itemCount;
            }

            await context.SaveChangesAsync();

            var totalPrice = items.Sum(x => x.Product.Price * x.Quantity);

            context.Orders.Update(items[0].Order);

            return Ok(new Basket
            {
                BasketId = basketId,
                Items = mapper.Map<List<ProductDto>>(items),
                TotalPrice = totalPrice
            });
        }

        [HttpGet("GetBasket/{basketId}")]
        public async Task<ActionResult<Basket>> GetBasket([FromRoute] Guid basketId)
        {
            var items = await context.OrdersItems
                                     .Include(x => x.Product)
                                     .Where(x => x.Order.Transaction.BasketId == basketId)
                                     .ToListAsync();

            var totalPrice = items.Sum(x => x.Product.Price * x.Quantity);
            

            return Ok(new Basket
            {
                BasketId = basketId,
                Items = mapper.Map<List<ProductDto>>(items),
                TotalPrice = totalPrice
            });
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<Basket>> CreateProduct([FromBody] Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ICollection<Product>>> GetAllProducts()
        {
            var products = await Mediator.Send(new GetAllProductsQuery { });
            return Ok(products);
        }

        [HttpGet("GetProductDetail/{productId}")]
        public async Task<ActionResult<Product>> GetProductDetail([FromRoute] int productId)
        {
            return Ok(await context.Products.Where(p => p.ProductId == productId).FirstAsync());
        }

        //TODO: finish order function
        [HttpPost("FinnishOrder")]
        public async Task<ActionResult<OrderDetail>> FinnishOrder([FromRoute] Guid basketId, [FromBody] Customer request)
        {
            return Ok(new OrderDetail { });
        }
    }
}
