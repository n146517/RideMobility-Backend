using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Data;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RiderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all riders (Admin only)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRiders() =>
            Ok(_context.Users.Where(u => u.Role == "Rider").ToList());
    }
}
