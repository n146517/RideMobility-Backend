using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Models;
using RideMobility.Api.Repositories.Interfaces;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _driverRepo;

        public DriverController(IDriverRepository driverRepo)
        {
            _driverRepo = driverRepo;
        }

        // Get all drivers (Admin only)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDrivers() =>
            Ok(await _driverRepo.GetAllAsync());

        // Get available drivers (Rider/Admin)
        [HttpGet("available")]
        [Authorize(Roles = "Rider,Admin")]
        public async Task<IActionResult> GetAvailableDrivers() =>
            Ok(await _driverRepo.GetAvailableDriversAsync());

        // Add a driver (Admin only)
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDriver([FromBody] Driver driver)
        {
            if (string.IsNullOrWhiteSpace(driver.Name))
                return BadRequest("Driver name is required");

            await _driverRepo.AddAsync(driver);
            await _driverRepo.SaveChangesAsync();
            return Ok(driver);
        }
    }
}
