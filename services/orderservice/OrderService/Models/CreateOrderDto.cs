using System;

namespace OrderService.Models;

public class CreateOrderDto
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
