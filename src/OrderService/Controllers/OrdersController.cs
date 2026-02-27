using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderContext _context;

    public OrdersController(OrderContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Orders.ToList());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Order order)
    {
        order.CreatedAt = DateTime.UtcNow;
        _context.Orders.Add(order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }
}
