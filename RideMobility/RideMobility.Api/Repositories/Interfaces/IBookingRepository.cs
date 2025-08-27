using RideMobility.Api.Models;

namespace RideMobility.Api.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<IEnumerable<Booking>> GetByRiderAsync(int riderId, int page, int pageSize);
        Task AddAsync(Booking booking);
        Task RemoveAsync(Booking booking);
        Task SaveChangesAsync();
    }
}
