using System.Collections.Generic;
using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Aps.Infrastructure
{
    public class ApsContext : DbContext
    {
        public DbSet<ApsSemiProduct> ApsSemiProducts { get; set; }
        public DbSet<ApsManufactureProcess> ApsManufactureProcesses { get; set; }
        public DbSet<ApsOrder> ApsOrders { get; set; }
        public DbSet<ApsResource> ApsResources { get; set; }
        public DbSet<ApsAssemblyProcess> ApsAssemblyProcesses { get; set; }
        public DbSet<ApsProduct> ApsProducts { get; set; }


        public DbSet<ApsAssemblyJob> ApsAssemblyJobs { get; set; }
        public DbSet<ApsManufactureJob> ApsManufactureJobs { get; set; }

        public DbSet<ResourceClass> ResourceClasses { get; set; }
        public DbSet<ProductInstance> ProductInstances { get; set; }
        public DbSet<SemiProductInstance> SemiProductInstances { get; set; }


        public DbSet<ScheduleRecord> ScheduleRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<ApsSemiProduct>()
                .HasMany(x => x.ApsManufactureProcesses)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApsManufactureProcess>()
                .HasOne(x => x.PrevPart)
                .WithOne()
                .HasForeignKey<ApsManufactureProcess>(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasKey(k => new {k.ApsAssemblyProcessId, k.ApsSemiProductId});

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasOne(x => x.ApsAssemblyProcess)
                .WithMany(x => x.InputSemiFinishedProducts)
                .HasForeignKey(x => x.ApsAssemblyProcessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasOne(x => x.ApsSemiProduct)
                .WithMany()
                .HasForeignKey(x => x.ApsSemiProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApsProductSemiProduct>()
                .HasKey(x => new {x.ApsSemiProductId, x.ApsProductId});

            modelBuilder.Entity<ApsProduct>()
                .HasMany(x => x.AssembleBySemiProducts)
                .WithOne(x => x.ApsProduct)
                .HasForeignKey(x => x.ApsProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApsSemiProduct>()
                .HasMany(x => x.ApsProductsFromRequisite)
                .WithOne(x => x.ApsSemiProduct)
                .HasForeignKey(x => x.ApsSemiProductId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ApsProduct>()
                .HasOne(x => x.ApsAssemblyProcess)
                .WithOne(p => p.OutputFinishedProduct)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ApsProcessResource>()
                .HasKey(x => new {x.ProcessId, x.ResourceClassId});

            modelBuilder.Entity<ApsProcess>()
                .HasMany(x => x.ApsResources)
                .WithOne(x => x.ApsProcess)
                .HasForeignKey(x => x.ProcessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResourceClass>()
                .HasMany<ApsProcessResource>()
                .WithOne(x => x.ResourceClass)
                .HasForeignKey(x => x.ResourceClassId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ResourceClassWithResource>()
                .HasKey(x => new {x.ResourceClassId, x.ApsResourceId});

            modelBuilder.Entity<ApsResource>()
                .HasMany(x => x.ResourceAttributes)
                .WithOne(x => x.ApsResource)
                .HasForeignKey(r => r.ApsResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResourceClass>()
                .HasMany(x => x.ApsResources)
                .WithOne(x => x.ResourceClass)
                .HasForeignKey(x => x.ResourceClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApsManufactureJob>()
                .HasOne(x => x.PreJob)
                .WithOne()
                .HasForeignKey<ApsManufactureJob>(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApsOrder>()
                .HasOne<ScheduleRecord>()
                .WithMany(x => x.Orders)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ScheduleRecord>()
                .HasMany(x => x.Jobs)
                .WithOne(x => x.ScheduleRecord)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScheduleRecord>()
                .HasMany(x => x.ApsAssemblyJobs)
                .WithOne(x => x.ScheduleRecord)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApsJob>()
                .HasMany(x => x.ApsResource)
                .WithMany(x => x.WorkJobs)
                .UsingEntity<Dictionary<string, object>>(
                "ApsJobApsResource",
                j => j
                    .HasOne<ApsResource>()
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<ApsJob>()
                    .WithMany()
                    .OnDelete(DeleteBehavior.Cascade)
                );
        }

        public ApsContext(DbContextOptions<ApsContext> options) : base(options)
        {
        }
    }
}