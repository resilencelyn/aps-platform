using Aps.Entity;
using Microsoft.EntityFrameworkCore;

namespace Aps.Infrastructure
{
    public class ApsContext : DbContext
    {
        public DbSet<ApsSemiProduct> ApsSemiProducts { get; set; }
        public DbSet<ApsManufactureProcess> ApsProcesses { get; set; }
        public DbSet<ApsOrder> ApsOrders { get; set; }
        public DbSet<ApsResource> ApsResources { get; set; }
        public DbSet<ApsAssemblyProcess> ApsAssemblyProcesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApsSemiProduct>()
                .HasMany(x => x.ApsManufactureProcesses)
                .WithOne()
                .HasForeignKey(p => p.PartId);
            modelBuilder.Entity<ApsManufactureProcess>()
                .HasOne(x => x.PrevPart)
                .WithOne()
                .HasForeignKey<ApsManufactureProcess>(x => x.PartId);

            modelBuilder.Entity<ApsResource>()
                .Property(x => x.Amount);
        }

        public ApsContext(DbContextOptions<ApsContext> options) : base(options)
        {
        }
    }
}