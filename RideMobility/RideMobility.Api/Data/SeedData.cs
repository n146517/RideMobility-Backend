using BCrypt.Net;
using RideMobility.Api.Models;

namespace RideMobility.Api.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Role = "Admin" },
                    new User { Username = "rider1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Rider@123"), Role = "Rider" },
                    new User { Username = "driver1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Driver@123"), Role = "Driver" }
                );
                context.SaveChanges();
            }

            if (!context.Drivers.Any())
            {
                context.Drivers.AddRange(
                    new Driver { Name = "Driver One", IsAvailable = true, VehicleType = VehicleType.Car },
                    new Driver { Name = "Driver Two", IsAvailable = true, VehicleType = VehicleType.Taxi },
                    new Driver { Name = "Driver Three", IsAvailable = true, VehicleType = VehicleType.ERiksha },
                    new Driver { Name = "Driver Four", IsAvailable = true, VehicleType = VehicleType.Auto }
                );
                context.SaveChanges();
            }
        }
    }
}
