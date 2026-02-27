using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using OrderService.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderContext _context;
    private readonly ICatalogClient _catalog;

    public OrdersController(OrderContext context, ICatalogClient catalog)
    {
        _context = context;
        _catalog = catalog;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var orders = _context.Orders.ToList();
        var result = new List<OrderResponse>();

        foreach (var o in orders)
        {
            var prod = await _catalog.GetProductAsync(o.ProductId);
            OrderService.Models.CategoryDto? cat = null;
            if (prod != null)
            {
                cat = await _catalog.GetCategoryAsync(prod.CategoryId);
            }

            result.Add(new OrderResponse
            {
                Id = o.Id,
                ProductId = o.ProductId,
                Quantity = o.Quantity,
                Total = o.Total,
                CreatedAt = o.CreatedAt,
                Product = prod,
                Category = cat
            });
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null) return NotFound();

        var prod = await _catalog.GetProductAsync(order.ProductId);
        CategoryDto? cat = null;
        if (prod != null)
            cat = await _catalog.GetCategoryAsync(prod.CategoryId);

        var resp = new OrderResponse
        {
            Id = order.Id,
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            Product = prod,
            Category = cat
        };

        return Ok(resp);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        // attempt to reserve stock in CatalogService
        var reserve = await _catalog.ReserveProductAsync(order.ProductId, order.Quantity);
        if (!reserve.Reserved)
        {
            if (reserve.StatusCode == 404)
                return BadRequest(new { error = "Product not found in CatalogService" });
            if (reserve.StatusCode == 409)
                return BadRequest(new { error = "Insufficient stock" });
            return BadRequest(new { error = "Unable to reserve product stock" });
        }

        var prod = reserve.Product!;
        // compute total from canonical product price
        order.Total = prod.Price * order.Quantity;
        order.CreatedAt = DateTime.UtcNow;

        _context.Orders.Add(order);
        _context.SaveChanges();

        var cat = await _catalog.GetCategoryAsync(prod.CategoryId);

        var resp = new OrderResponse
        {
            Id = order.Id,
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            Product = prod,
            Category = cat
        };

        return CreatedAtAction(nameof(Get), new { id = order.Id }, resp);
    }
}
