namespace TransactionService.Models
{
    // minimal shape matching OrderService responses used for validation
    public class OrderDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
