using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideMobility.Api.Models;
using RideMobility.Api.Repositories.Interfaces;
using RideMobility.Api.Services.Helpers;

namespace RideMobility.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Rider,Admin")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IDriverRepository _driverRepo;
        private readonly IRideRequestRepository _rideRequestRepo;

        public BookingController(
            IBookingRepository bookingRepo,
            IDriverRepository driverRepo,
            IRideRequestRepository rideRequestRepo)
        {
            _bookingRepo = bookingRepo;
            _driverRepo = driverRepo;
            _rideRequestRepo = rideRequestRepo;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] Booking booking)
        {
            if (booking.RideRequestId <= 0)
                return BadRequest("RideRequestId required");
            var rideRequest = await _rideRequestRepo.GetByIdAsync(booking.RideRequestId);
            if (rideRequest == null) return NotFound("Ride request not found");
            if (rideRequest.IsCompleted) return BadRequest("Ride already completed");

            var driver = (await _driverRepo.GetAvailableDriversAsync(rideRequest.VehicleType)).FirstOrDefault();
            if (driver == null) return BadRequest($"No available {rideRequest.VehicleType} drivers");

            booking.DriverId = driver.Id;
            booking.VehicleType = driver.VehicleType;
            booking.Fare = FareCalculator.CalculateFare(rideRequest.DistanceKm);

            rideRequest.IsCompleted = true;
            driver.IsAvailable = false;

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            return Ok(booking);
        }

        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);
            if (booking == null) return NotFound("Booking not found");
            if (booking.RideRequest.IsCompleted)
                return BadRequest("Cannot cancel completed ride");

            booking.RideRequest.IsCompleted = false;
            booking.Driver.IsAvailable = true;

            await _bookingRepo.RemoveAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            return Ok(new { message = "Booking cancelled successfully" });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookings() =>
            Ok(await _bookingRepo.GetAllAsync());

        [HttpGet("byrider/{riderId}")]
        [Authorize(Roles = "Rider,Admin")]
        public async Task<IActionResult> GetBookingsByRider(int riderId, int page = 1, int pageSize = 10) =>
            Ok(await _bookingRepo.GetByRiderAsync(riderId, page, pageSize));
    }
}
