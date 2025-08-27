using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Data;
using RideMobility.Api.Models;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all drivers (Admin only)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDrivers() => Ok(_context.Drivers.ToList());

        // Get available drivers (Rider/Admin)
        [HttpGet("available")]
        [Authorize(Roles = "Rider,Admin")]
        public IActionResult GetAvailableDrivers() =>
            Ok(_context.Drivers.Where(d => d.IsAvailable).ToList());

        // Add a driver (Admin only)
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddDriver([FromBody] Driver driver)
        {
            if (string.IsNullOrWhiteSpace(driver.Name))
                return BadRequest("Driver name is required");

            _context.Drivers.Add(driver);
            _context.SaveChanges();
            return Ok(driver);
        }
    }
}
