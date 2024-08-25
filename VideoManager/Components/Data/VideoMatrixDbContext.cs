using Microsoft.EntityFrameworkCore;
using VideoMatrix.Models;

namespace VideoMatrix.Data
{
    public class VideoMatrixDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        public VideoMatrixDbContext(DbContextOptions<VideoMatrixDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .HasDiscriminator<string>("DeviceType")
                .HasValue<Transmitter>("Transmitter")
                .HasValue<Receiver>("Receiver");

            // Seed data
            modelBuilder.Entity<Transmitter>().HasData(
                new Transmitter { Id = 1, Name = "Transmitter 1", IpAddress = "192.168.1.1", Status = DeviceStatus.On, ImageUrl = "https://via.placeholder.com/150" },
                new Transmitter { Id = 2, Name = "Transmitter 2", IpAddress = "192.168.1.2", Status = DeviceStatus.Standby, ImageUrl = "https://via.placeholder.com/150" }
            );

            modelBuilder.Entity<Receiver>().HasData(
                new Receiver { Id = 3, Name = "Receiver 1", IpAddress = "192.168.1.3", Status = DeviceStatus.Off, ImageUrl = "https://via.placeholder.com/150" },
                new Receiver { Id = 4, Name = "Receiver 2", IpAddress = "192.168.1.4", Status = DeviceStatus.On, ImageUrl = "https://via.placeholder.com/150" }
            );

            modelBuilder.Entity<Profile>().HasData(
                new Profile { Id = 1, Name = "Default Profile" }
            );
        }
    }
}
