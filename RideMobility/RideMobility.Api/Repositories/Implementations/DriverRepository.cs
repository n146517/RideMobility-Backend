using Microsoft.EntityFrameworkCore;
using RideMobility.Api.Data;
using RideMobility.Api.Models;
using RideMobility.Api.Repositories.Interfaces;

namespace RideMobility.Api.Repositories.Implementations
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Driver>> GetAllAsync() =>
            await _context.Drivers.ToListAsync();

        public async Task<IEnumerable<Driver>> GetAvailableDriversAsync() =>
            await _context.Drivers.Where(d => d.IsAvailable).ToListAsync();

        public async Task AddAsync(Driver driver) =>
            await _context.Drivers.AddAsync(driver);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
