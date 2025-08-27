using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Repositories.Interfaces;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiderController : ControllerBase
    {
        private readonly IRiderRepository _riderRepo;

        public RiderController(IRiderRepository riderRepo)
        {
            _riderRepo = riderRepo;
        }

        // Get all riders (Admin only)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRiders() =>
            Ok(await _riderRepo.GetAllRidersAsync());
    }
}
