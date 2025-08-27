using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideMobility.Api.Data;
using RideMobility.Api.Models;
using RideMobility.Api.Services.Helpers;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Rider,Admin")]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("book")]
        public IActionResult BookRide([FromBody] Booking booking)
        {
            if (booking.RideRequestId <= 0)
                return BadRequest("RideRequestId required");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var ride = _context.RideRequests
                    .Include(r => r.Rider)
                    .FirstOrDefault(r => r.Id == booking.RideRequestId);

                if (ride == null) return NotFound("Ride not found");
                if (ride.IsCompleted) return BadRequest("Ride already completed");

                // Assign nearest available driver
                var driver = _context.Drivers.Where(d => d.IsAvailable).OrderBy(d => d.Id).FirstOrDefault();
                if (driver == null) return BadRequest("No available drivers");

                booking.DriverId = driver.Id;
                booking.Driver = driver;
                booking.Fare = FareCalculator.CalculateFare(ride.DistanceKm);

                ride.IsCompleted = true;
                driver.IsAvailable = false;

                _context.Bookings.Add(booking);
                _context.SaveChanges();
                transaction.Commit();

                return Ok(booking);
            }
            catch
            {
                transaction.Rollback();
                return StatusCode(500, "Error booking ride");
            }
        }

        [HttpPost("cancel/{bookingId}")]
        public IActionResult CancelBooking(int bookingId)
        {
            var booking = _context.Bookings
                .Include(b => b.Driver)
                .Include(b => b.RideRequest)
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking == null) return NotFound("Booking not found");
            if (booking.RideRequest.IsCompleted) return BadRequest("Cannot cancel completed ride");

            booking.RideRequest.IsCompleted = false;
            booking.Driver.IsAvailable = true;

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return Ok(new { message = "Booking cancelled successfully" });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBookings() => Ok(_context.Bookings.ToList());

        [HttpGet("byrider/{riderId}")]
        [Authorize(Roles = "Rider,Admin")]
        public IActionResult GetBookingsByRider(int riderId, int page = 1, int pageSize = 10)
        {
            var bookings = _context.Bookings
                .Include(b => b.RideRequest)
                .Include(b => b.Driver)
                .Where(b => b.RideRequest.RiderId == riderId)
                .OrderByDescending(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Ok(bookings);
        }
    }
}
