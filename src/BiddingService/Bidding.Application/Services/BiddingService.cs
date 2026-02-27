using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bidding.Application.Interfaces;
using Bidding.Domain.Entities;

namespace Bidding.Application.Services
{
    public class BiddingService
    {
        private readonly IAuctionRepository _repo;

        public BiddingService(IAuctionRepository repo)
        {
            _repo = repo;
        }

        public Task<Auction> CreateAuctionAsync(Auction auction)
        {
            // Basic validation
            if (auction.EndDate <= auction.StartDate) throw new ArgumentException("EndDate must be after StartDate");
            return _repo.CreateAsync(auction);
        }

        public Task<IEnumerable<Auction>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Auction?> GetAsync(Guid id) => _repo.GetAsync(id);

        public async Task PlaceBidAsync(Bid bid)
        {
            var auction = await _repo.GetAsync(bid.AuctionId);
            if (auction == null) throw new InvalidOperationException("Auction not found");

            // Let domain entity perform validation
            auction.AddBid(bid);

            await _repo.PlaceBidAsync(bid);
        }
    }
}
