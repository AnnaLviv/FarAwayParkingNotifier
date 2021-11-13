using FarAwayParkingNotifier.Domain;
using Microsoft.EntityFrameworkCore;

namespace FarAwayParkingNotifier.Repository
{
    public class CarDeviceDbContext : DbContext
    {
        private readonly string connectionString;

        public CarDeviceDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public CarDeviceDbContext(DbContextOptions options) : base(options)
        {    }

        public DbSet<CarDeviceMapping> CarDeviceMappings { get; set; }
        public DbSet<SignalLocation> SignalLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarDeviceMapping>().HasIndex(mapping => mapping.CarId).IsUnique();
            modelBuilder.Entity<CarDeviceMapping>().HasIndex(mapping => mapping.DeviceId).IsUnique();
            modelBuilder.Entity<CarDeviceMapping>().HasKey(mapping => new { mapping.CarId, mapping.DeviceId });

            modelBuilder.Entity<SignalLocation>().HasIndex(signalLocation => signalLocation.SignalSourceId).IsUnique();
            modelBuilder.Entity<SignalLocation>().HasKey(signalLocation => new { signalLocation.SignalSourceId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
            }

        }
    }
}