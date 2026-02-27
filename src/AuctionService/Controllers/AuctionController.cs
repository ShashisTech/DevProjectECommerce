using Microsoft.AspNetCore.Mvc;
using AuctionService.Data;
using AuctionService.Models;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly AuctionContext _context;

    public AuctionController(AuctionContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.AuctionItems.ToList());
    }

    [HttpPost]
    public IActionResult Post([FromBody] AuctionItem item)
    {
        _context.AuctionItems.Add(item);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }
}
