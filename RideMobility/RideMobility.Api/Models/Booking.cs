using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideMobility.Api.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RideRequestId { get; set; }

        [ForeignKey("RideRequestId")]
        public RideRequest RideRequest { get; set; }

        [Required]
        public int DriverId { get; set; }

        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }

        [Required]
        public VehicleType VehicleType { get; set; }

        public decimal Fare { get; set; }
    }
}
