using Aps.Shared.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApsSemiProduct>()
                .HasMany(x => x.ApsManufactureProcesses)
                .WithOne();

            modelBuilder.Entity<ApsManufactureProcess>()
                .HasOne(x => x.PrevPart)
                .WithOne()
                .HasForeignKey<ApsManufactureProcess>(x => x.Id);

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasKey(k => new { k.ApsAssemblyProcessId, k.ApsSemiProductId });

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasOne(x => x.ApsAssemblyProcess)
                .WithMany(x => x.InputSemiFinishedProducts)
                .HasForeignKey(x => x.ApsAssemblyProcessId);

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasOne(x => x.ApsSemiProduct)
                .WithMany()
                .HasForeignKey(x => x.ApsSemiProductId);

            modelBuilder.Entity<ApsProductSemiProduct>()
                .HasKey(x => new { x.ApsSemiProductId, ApsProductId = x.ProductId });


            modelBuilder.Entity<ApsProduct>()
                .HasOne(x => x.ApsAssemblyProcess)
                .WithOne(p => p.OutputFinishedProduct);


            modelBuilder.Entity<ApsProcessResource>()
                .HasKey(x => new { ApsProcessId = x.ProcessId, x.ResourceClassId });

            modelBuilder.Entity<ApsProcess>()
                .HasMany(x => x.ApsResources)
                .WithOne(x => x.ApsProcess)
                .HasForeignKey(x => x.ProcessId);

            modelBuilder.Entity<ResourceClass>()
                .HasMany<ApsProcessResource>()
                .WithOne(x => x.ResourceClass)
                .HasForeignKey(x => x.ResourceClassId);


            modelBuilder.Entity<ResourceClassWithResource>()
                .HasKey(x => new { x.ResourceClassId, x.ApsResourceId });

            modelBuilder.Entity<ApsResource>()
                .HasMany(x => x.ResourceAttributes)
                .WithOne(x => x.ApsResource)
                .HasForeignKey(r => r.ApsResourceId);

            modelBuilder.Entity<ResourceClass>()
                .HasMany(x => x.ApsResources)
                .WithOne(x => x.ResourceClass)
                .HasForeignKey(x => x.ResourceClassId);

            var list = "product_semi_d,product_semi_o," +
                       "product_semi_a,product_semi_j," +
                       "product_semi_r,product_semi_f";
            var allSemiProductString =
                "product_semi_d,product_semi_o,product_semi_a,product_semi_j,product_semi_r,product_semi_f," +
                "product_semi_s,product_semi_e,product_semi_o,product_semi_n, product_semi_f, product_semi_a," +
                "product_semi_p,product_semi_g,product_semi_s,product_semi_j, product_semi_d," +
                "product_semi_f,product_semi_s,product_semi_n,product_semi_c,product_semi_t,product_semi_e," +
                "product_semi_f,product_semi_l,product_semi_q,product_semi_g,product_semi_d,product_semi_j,";

            var allProductString =
                "product_1,product_1,product_1,product_1,product_1,product_1,product_1,product_2,product_2,product_2,product_2,product_2,product_2,product_2,product_3,product_3,product_3,product_3,product_3,product_3,product_4,product_4,product_4,product_4,product_4,product_4,product_4,product_5,product_5,product_5,product_5,product_5,product_5,product_5,product_6,product_6,product_6";

            var allProduct = allProductString.Split(',')
                .Select(x => x.Trim())
                .ToHashSet()
                .Select(x => new ApsProduct()
                {
                    Id = x,
                });


            var allSemiProduct = allSemiProductString.Split(',').ToHashSet().Select(x => new ApsSemiProduct()
            {
                Id = x,
            });

            modelBuilder.Entity<ApsProduct>()
                .HasData(allProduct);

            modelBuilder.Entity<ApsSemiProduct>()
                .HasData(allSemiProduct);


            modelBuilder.Entity<ApsAssemblyProcess>()
                .HasData(new List<ApsAssemblyProcess>()
                {
                    new ApsAssemblyProcess()
                    {
                        Id = "process_end_A",
                        PartName = "process_end_A",
                        ProductionMode = ProductionMode.Sp,
                        MinimumProductionQuantity = 1,
                        MaximumProductionQuantity = 1,
                        Workspace = Workspace.装配,
                        ProductionTime = TimeSpan.FromMinutes(4),
                    }
                });

            modelBuilder.Entity<ApsManufactureProcess>()
                .HasData(new List<ApsManufactureProcess>()
                {
                    new ApsManufactureProcess()
                    {
                        Id = "process_1_a",
                        PartName = "process_1_a",
                        ProductionMode = ProductionMode.Sp,
                        MaximumProductionQuantity = 1,
                        MinimumProductionQuantity = 1,
                        Workspace = Workspace.加工,
                        ProductionTime = new TimeSpan(0, 0, 1),
                    }
                });

            var apsAssemblyProcessSemiProducts = list.Split(',')
                .Select(x => x.Trim())
                .Select(x => new ApsAssemblyProcessSemiProduct()
                {
                    Amount = 1,
                    ApsSemiProductId = x,
                    ApsAssemblyProcessId = "process_end_A",
                }).ToList();

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasData(apsAssemblyProcessSemiProducts);
            //
            //
            // modelBuilder.Entity<ApsProcessResource>()
            //     .HasData(new List<ApsProcessResource>()
            //     {
            //         new ApsProcessResource()
            //         {
            //             Amount = 3,
            //             ProcessId = "process_end_A",
            //             // ResourceClass = "机床",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 2,
            //             ProcessId = "process_end_A",
            //             // ResourceClass = "高级机床",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 1,
            //             ProcessId = "process_end_A",
            //             // ResourceClass = "人员",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 1,
            //             // ResourceClass = "高级人员",
            //             ProcessId = "process_end_A",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 3,
            //             // ResourceClass = "设备",
            //             ProcessId = "process_end_A",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 2,
            //             // ResourceClass = "高级设备",
            //             ProcessId = "process_end_A",
            //         },
            //
            //         new ApsProcessResource()
            //         {
            //             Amount = 3,
            //             ProcessId = "process_1_a",
            //             // ResourceClass = "机床",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 2,
            //             ProcessId = "process_1_a",
            //             // ResourceClass = "高级机床",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 1,
            //             ProcessId = "process_1_a",
            //             // ResourceClass = "人员",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 1,
            //             // ResourceClass = "高级人员",
            //             ProcessId = "process_1_a",
            //         },
            //         new ApsProcessResource()
            //         {
            //             // ResourceClass = "设备",
            //             ProcessId = "process_1_a",
            //         },
            //         new ApsProcessResource()
            //         {
            //             Amount = 2,
            //             // ResourceClass = "高级设备",
            //             ProcessId = "process_1_a",
            //         }
            //     });
        }

        public ApsContext(DbContextOptions<ApsContext> options) : base(options)
        {
        }
    }
}