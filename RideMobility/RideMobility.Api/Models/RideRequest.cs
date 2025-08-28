using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideMobility.Api.Models
{
    public class RideRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RiderId { get; set; }

        [ForeignKey("RiderId")]
        public User Rider { get; set; }

        [Required, MaxLength(100)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string DropLocation { get; set; } = string.Empty;

        [Required]
        public double DistanceKm { get; set; } = 0;

        public bool IsCompleted { get; set; } = false;

        [Required]
        public VehicleType VehicleType { get; set; } = VehicleType.Car; // Rider's choice
    }
}
