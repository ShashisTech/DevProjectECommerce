using System;

namespace OrderService.Models
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProductDto? Product { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
