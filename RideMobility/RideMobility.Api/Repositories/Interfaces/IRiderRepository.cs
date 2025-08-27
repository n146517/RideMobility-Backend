using RideMobility.Api.Models;

namespace RideMobility.Api.Repositories.Interfaces
{
    public interface IRiderRepository
    {
        Task<IEnumerable<User>> GetAllRidersAsync();
    }
}
