using System;

namespace UserService.Models;

public class CreateUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}
