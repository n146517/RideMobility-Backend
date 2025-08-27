using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Data;
using RideMobility.Api.Models;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RideController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("request")]
        [Authorize(Roles = "Rider")]
        public IActionResult CreateRide([FromBody] RideRequest ride)
        {
            if (ride.RiderId <= 0) return BadRequest("RiderId required");
            if (string.IsNullOrWhiteSpace(ride.PickupLocation) || string.IsNullOrWhiteSpace(ride.DropLocation))
                return BadRequest("Pickup and drop locations required");
            if (ride.DistanceKm <= 0) return BadRequest("Distance must be greater than 0");

            var riderExists = _context.Users.Any(u => u.Id == ride.RiderId && u.Role == "Rider");
            if (!riderExists) return NotFound("Rider not found");

            _context.RideRequests.Add(ride);
            _context.SaveChanges();
            return Ok(ride);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRides() => Ok(_context.RideRequests.ToList());

        [HttpGet("byrider/{riderId}")]
        [Authorize(Roles = "Rider,Admin")]
        public IActionResult GetRidesByRider(int riderId, int page = 1, int pageSize = 10)
        {
            var rides = _context.RideRequests
                .Where(r => r.RiderId == riderId)
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Ok(rides);
        }
    }
}
