using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bidding.Domain.Entities;

namespace Bidding.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task<Auction> CreateAsync(Auction auction);
        Task<Auction?> GetAsync(Guid id);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task PlaceBidAsync(Bid bid);
    }
}
