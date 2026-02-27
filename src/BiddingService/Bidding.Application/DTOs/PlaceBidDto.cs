using System;

namespace Bidding.Application.DTOs
{
    public class PlaceBidDto
    {
        public string Bidder { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
