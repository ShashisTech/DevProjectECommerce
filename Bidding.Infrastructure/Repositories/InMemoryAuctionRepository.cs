using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bidding.Application.Interfaces;
using Bidding.Domain.Entities;

namespace Bidding.Infrastructure.Repositories
{
    public class InMemoryAuctionRepository : IAuctionRepository
    {
        private readonly ConcurrentDictionary<Guid, Auction> _store = new();

        public Task<Auction> CreateAsync(Auction auction)
        {
            _store[auction.Id] = auction;
            return Task.FromResult(auction);
        }

        public Task<IEnumerable<Auction>> GetAllAsync()
        {
            return Task.FromResult(_store.Values.AsEnumerable());
        }

        public Task<Auction?> GetAsync(Guid id)
        {
            _store.TryGetValue(id, out var auction);
            return Task.FromResult(auction);
        }

        public Task PlaceBidAsync(Bid bid)
        {
            if (!_store.TryGetValue(bid.AuctionId, out var auction)) throw new InvalidOperationException("Auction not found");
            // Add bid in the auction instance (domain) was already done in service, but ensure persistence
            auction.AddBid(bid);
            _store[auction.Id] = auction;
            return Task.CompletedTask;
        }
    }
}
