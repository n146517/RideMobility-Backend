using Microsoft.EntityFrameworkCore;
using RideMobility.Api.Data;
using RideMobility.Api.Models;
using RideMobility.Api.Repositories.Interfaces;

namespace RideMobility.Api.Repositories.Implementations
{
    public class RiderRepository : IRiderRepository
    {
        private readonly ApplicationDbContext _context;

        public RiderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllRidersAsync() =>
            await _context.Users
                .Where(u => u.Role == "Rider")
                .ToListAsync();
    }
}
