using System;

namespace Bidding.Domain.Entities
{
    public class Bid
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AuctionId { get; set; }
        public string Bidder { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
    }
}
