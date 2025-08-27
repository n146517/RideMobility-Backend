using Microsoft.EntityFrameworkCore;
using RideMobility.Api.Data;
using RideMobility.Api.Models;
using RideMobility.Api.Repositories.Interfaces;

namespace RideMobility.Api.Repositories.Implementations
{
    public class RideRequestRepository : IRideRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RideRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RideRequest ride) =>
            await _context.RideRequests.AddAsync(ride);

        public async Task<IEnumerable<RideRequest>> GetAllAsync() =>
            await _context.RideRequests.ToListAsync();

        public async Task<IEnumerable<RideRequest>> GetByRiderAsync(int riderId, int page, int pageSize) =>
            await _context.RideRequests
                .Where(r => r.RiderId == riderId)
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<RideRequest?> GetByIdAsync(int id) =>
            await _context.RideRequests.FindAsync(id);

        public async Task<bool> RiderExistsAsync(int riderId) =>
            await _context.Users.AnyAsync(u => u.Id == riderId && u.Role == "Rider");

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
