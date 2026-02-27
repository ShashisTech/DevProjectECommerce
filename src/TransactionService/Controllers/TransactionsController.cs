using Microsoft.AspNetCore.Mvc;
using TransactionService.Data;
using TransactionService.Models;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionContext _context;

    public TransactionsController(TransactionContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Transactions.ToList());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Transaction tx)
    {
        tx.ProcessedAt = DateTime.UtcNow;
        _context.Transactions.Add(tx);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = tx.Id }, tx);
    }
}
