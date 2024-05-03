using jAPS.API.Commands;
using jAPS.API.Models;
using jAPS.API.Queries;
using Microsoft.AspNetCore.Mvc;

namespace jAPS.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainController : ApiControllerBase
    {  
        [HttpPost("AddToBasket")]
        public async Task<ActionResult<Basket>> AddToBasket([FromBody] Basket request)
        {
            var basket = await Mediator.Send(new AddToBasketCommand { Basket = request });

            return Ok(basket);
        }

        [HttpPost("RemoveFromBasket/{basketId}")]
        public async Task<ActionResult<Basket>> RemoveFromBasket([FromRoute] Guid basketId, [FromBody] ProductDto request)
        {
            var basket = await Mediator.Send(new RemoveFromBasketCommand
            {
                BasketId = basketId,
                Product = request
            });

            return Ok(basket);
        }

        [HttpGet("GetBasket/{basketId}")]
        public async Task<ActionResult<Basket>> GetBasket([FromRoute] Guid basketId)
        {
            var basket = await Mediator.Send(new GetBasketQuery
            {
                basketId = basketId
            });

            return Ok(basket);
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            var prod = await Mediator.Send(new CreateProductCommand
            {
                Product = product
            });

            return Ok(prod);
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
            var product = await Mediator.Send(new GetProductQuery
            {
                ProductId = productId
            });

            return Ok(product);
        }

        [HttpPost("FinnishOrder/{basketId}")]
        public async Task<ActionResult<OrderDetail>> FinnishOrder([FromRoute] Guid basketId, [FromBody] Customer request)
        {
            var orderDetail = await Mediator.Send(new FinishOrderCommand
            {
                BasketId = basketId,
                Customer = request
            });

            return Ok(orderDetail);
        }
    }
}