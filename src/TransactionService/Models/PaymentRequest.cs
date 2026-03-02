namespace TransactionService.Models
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // CashOnDelivery, Card, UPI
    }
}
