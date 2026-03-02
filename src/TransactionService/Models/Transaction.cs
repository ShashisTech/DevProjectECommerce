namespace TransactionService.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }

        // Payment method chosen by user: CashOnDelivery, Card, UPI, etc.
        public string PaymentMethod { get; set; } = string.Empty;

        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
