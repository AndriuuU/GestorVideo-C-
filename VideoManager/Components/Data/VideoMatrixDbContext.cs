using Microsoft.EntityFrameworkCore;
using VideoMatrix.Models;

namespace VideoMatrix.Data
{
    public class VideoMatrixDbContext : DbContext
    {
        public VideoMatrixDbContext(DbContextOptions<VideoMatrixDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .HasDiscriminator<string>("DeviceType")
                .HasValue<Transmitter>("Transmitter")
                .HasValue<Receiver>("Receiver");

            base.OnModelCreating(modelBuilder);
        }
    }
}
