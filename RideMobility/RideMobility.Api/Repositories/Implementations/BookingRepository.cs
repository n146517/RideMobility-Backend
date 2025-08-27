using Microsoft.EntityFrameworkCore;
using RideMobility.Api.Data;
using RideMobility.Api.Models;
using RideMobility.Api.Repositories.Interfaces;

namespace RideMobility.Api.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int bookingId) =>
            await _context.Bookings
                .Include(b => b.Driver)
                .Include(b => b.RideRequest) // ✅ Fixed: was "Ride"
                .FirstOrDefaultAsync(b => b.Id == bookingId);

        public async Task<IEnumerable<Booking>> GetAllAsync() =>
            await _context.Bookings
                .Include(b => b.RideRequest) // ✅ Fixed
                .Include(b => b.Driver)
                .ToListAsync();

        public async Task<IEnumerable<Booking>> GetByRiderAsync(int riderId, int page, int pageSize) =>
            await _context.Bookings
                .Include(b => b.RideRequest) // ✅ Fixed
                .Include(b => b.Driver)
                .Where(b => b.RideRequest.RiderId == riderId) // ✅ Fixed: was "Ride"
                .OrderByDescending(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task AddAsync(Booking booking) =>
            await _context.Bookings.AddAsync(booking);

        public Task RemoveAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            return Task.CompletedTask; // ✅ fixed signature
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
