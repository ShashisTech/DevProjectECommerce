using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Dtos;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserContext _context;

    public UsersController(UserContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var u = await _context.Users.FindAsync(id);
        if (u == null) return NotFound();
        var dto = new UserDto { Id = u.Id, Username = u.Username, Email = u.Email, FirstName = u.FirstName, LastName = u.LastName, IsSeller = u.IsSeller, IsActive = u.IsActive, CreatedAt = u.CreatedAt };
        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserDto update)
    {
        var u = await _context.Users.FindAsync(id);
        if (u == null) return NotFound();
        u.FirstName = update.FirstName;
        u.LastName = update.LastName;
        u.IsSeller = update.IsSeller;
        u.IsActive = update.IsActive;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int limit = 50)
    {
        var users = await _context.Users.Take(limit).ToListAsync();
        var dtos = users.Select(u => new UserDto { Id = u.Id, Username = u.Username, Email = u.Email, FirstName = u.FirstName, LastName = u.LastName, IsSeller = u.IsSeller, IsActive = u.IsActive, CreatedAt = u.CreatedAt });
        return Ok(dtos);
    }
}
