using Microsoft.AspNetCore.Mvc;
using TransactionService.Data;
using TransactionService.Models;
using TransactionService.Services;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionContext _context;
    private readonly IOrderClient _orderClient;

    public TransactionsController(TransactionContext context, IOrderClient orderClient)
    {
        _context = context;
        _orderClient = orderClient;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Transactions.ToList());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentRequest request)
    {
        if (request.OrderId <= 0 || string.IsNullOrWhiteSpace(request.PaymentMethod))
        {
            return BadRequest(new { error = "OrderId and PaymentMethod are required." });
        }

        // synchronous-style call to OrderService (over HTTP)
        var order = await _orderClient.GetOrderAsync(request.OrderId);
        if (order == null)
        {
            return NotFound(new { error = $"Order {request.OrderId} not found in OrderService." });
        }

        var allowed = new[] { "CashOnDelivery", "Card", "UPI" };
        if (!allowed.Contains(request.PaymentMethod, StringComparer.OrdinalIgnoreCase))
        {
            return BadRequest(new { error = "Unsupported payment method. Allowed: CashOnDelivery, Card, UPI." });
        }

        var tx = new Transaction
        {
            OrderId = order.Id,
            Amount = order.Total,
            PaymentMethod = request.PaymentMethod,
            Status = "Succeeded",
            ProcessedAt = DateTime.UtcNow
        };

        _context.Transactions.Add(tx);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = tx.Id }, tx);
    }
}
