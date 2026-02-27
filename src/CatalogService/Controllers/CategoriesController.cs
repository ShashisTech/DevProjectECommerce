using Microsoft.AspNetCore.Mvc;
using CatalogService.Data;
using CatalogService.Models;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CatalogContext _context;

    public CategoriesController(CatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Categories.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var c = _context.Categories.Find(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }
}