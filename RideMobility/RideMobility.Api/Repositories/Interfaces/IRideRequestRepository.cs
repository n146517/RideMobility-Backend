using RideMobility.Api.Models;

namespace RideMobility.Api.Repositories.Interfaces
{
    public interface IRideRequestRepository
    {
        Task AddAsync(RideRequest ride);
        Task<IEnumerable<RideRequest>> GetAllAsync();
        Task<IEnumerable<RideRequest>> GetByRiderAsync(int riderId, int page, int pageSize);
        Task<RideRequest?> GetByIdAsync(int id);
        Task<bool> RiderExistsAsync(int riderId);
        Task SaveChangesAsync();
    }
}
