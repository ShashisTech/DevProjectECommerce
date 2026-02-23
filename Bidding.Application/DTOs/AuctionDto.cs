using System;
using System.Collections.Generic;
using Bidding.Domain.Entities;

namespace Bidding.Application.DTOs
{
    public class AuctionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StartingPrice { get; set; }
        public IEnumerable<Bid> Bids { get; set; } = Array.Empty<Bid>();
    }
}
