using RideMobility.Api.Models;

namespace RideMobility.Api.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        Task<IEnumerable<Driver>> GetAllAsync();
        Task<IEnumerable<Driver>> GetAvailableDriversAsync(VehicleType? vehicleType = null);
        Task AddAsync(Driver driver);
        Task SaveChangesAsync();
    }
}
