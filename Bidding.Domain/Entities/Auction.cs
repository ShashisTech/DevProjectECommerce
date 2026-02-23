using System;
using System.Collections.Generic;
using System.Linq;

namespace Bidding.Domain.Entities
{
    public class Auction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public decimal StartingPrice { get; set; }

        private readonly List<Bid> _bids = new();
        public IReadOnlyCollection<Bid> Bids => _bids.AsReadOnly();

        public decimal HighestBidAmount => _bids.Count == 0 ? StartingPrice : _bids.Max(b => b.Amount);

        public void AddBid(Bid bid)
        {
            if (bid.AuctionId != Id) throw new InvalidOperationException("Bid belongs to a different auction.");
            if (DateTime.UtcNow < StartDate) throw new InvalidOperationException("Auction has not started.");
            if (DateTime.UtcNow > EndDate) throw new InvalidOperationException("Auction has ended.");
            if (bid.Amount <= HighestBidAmount) throw new InvalidOperationException("Bid amount must be greater than current highest bid.");

            _bids.Add(bid);
        }
    }
}
