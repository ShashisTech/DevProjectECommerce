using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserValidatorService _userValidator;
        private readonly ProductValidatorService _productValidator;

        public OrdersController(AppDbContext context,
        UserValidatorService userValidator,
        ProductValidatorService productValidator)
        {
            _context = context;
            _userValidator = userValidator;
            _productValidator = productValidator;

        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders.AsNoTracking().ToListAsync();
            return Ok(orders);
        }

        // Create order and validate user exists via userservice
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {

            var userExists = await _userValidator.UserExists(dto.UserId);

            if (!userExists)
                return BadRequest("User does not exist");

            var productExists = await _productValidator.ProductExists(dto.ProductId);

            if (!productExists)
                return BadRequest("Product does not exists");

            var order = new Order
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}
