using Microsoft.EntityFrameworkCore;
using RideMobility.Api.Models;

namespace RideMobility.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<RideRequest> RideRequests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
