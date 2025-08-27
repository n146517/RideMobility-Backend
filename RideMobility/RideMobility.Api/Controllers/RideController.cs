using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Models;
using RideMobility.Api.Repositories.Interfaces;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly IRideRequestRepository _rideRepo;

        public RideController(IRideRequestRepository rideRepo)
        {
            _rideRepo = rideRepo;
        }

        [HttpPost("request")]
        [Authorize(Roles = "Rider")]
        public async Task<IActionResult> CreateRide([FromBody] RideRequest ride)
        {
            if (ride.RiderId <= 0) return BadRequest("RiderId required");
            if (string.IsNullOrWhiteSpace(ride.PickupLocation) || string.IsNullOrWhiteSpace(ride.DropLocation))
                return BadRequest("Pickup and drop locations required");
            if (ride.DistanceKm <= 0) return BadRequest("Distance must be greater than 0");

            var riderExists = await _rideRepo.RiderExistsAsync(ride.RiderId);
            if (!riderExists) return NotFound("Rider not found");

            await _rideRepo.AddAsync(ride);
            await _rideRepo.SaveChangesAsync();

            return Ok(ride);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRides() =>
            Ok(await _rideRepo.GetAllAsync());

        [HttpGet("byrider/{riderId}")]
        [Authorize(Roles = "Rider,Admin")]
        public async Task<IActionResult> GetRidesByRider(int riderId, int page = 1, int pageSize = 10) =>
            Ok(await _rideRepo.GetByRiderAsync(riderId, page, pageSize));
    }
}
