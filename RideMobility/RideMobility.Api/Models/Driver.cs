using System.ComponentModel.DataAnnotations;

namespace RideMobility.Api.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
    }
}
