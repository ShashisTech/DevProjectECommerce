using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bidding.Application.Interfaces;
using Bidding.Domain.Entities;

namespace Bidding.Infrastructure.Repositories;

public class EfAuctionRepository : IAuctionRepository
{
    private readonly BiddingDbContext _db;

    public EfAuctionRepository(BiddingDbContext db) => _db = db;

    public async Task<Auction> CreateAsync(Auction auction)
    {
        _db.Auctions.Add(auction);
        await _db.SaveChangesAsync();
        return auction;
    }

    public async Task<IEnumerable<Auction>> GetAllAsync()
    {
        return await _db.Auctions.Include(a => a.Bids).ToListAsync();
    }

    public async Task<Auction?> GetAsync(Guid id)
    {
        return await _db.Auctions.Include(a => a.Bids).FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task PlaceBidAsync(Bid bid)
    {
        var auction = await _db.Auctions.Include(a => a.Bids).FirstOrDefaultAsync(a => a.Id == bid.AuctionId);
        if (auction is null) throw new InvalidOperationException("Auction not found");
        auction.AddBid(bid);
        await _db.SaveChangesAsync();
    }
}
