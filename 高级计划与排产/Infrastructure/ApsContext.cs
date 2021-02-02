using System;
using System.Collections.Generic;
using System.Linq;
using Aps.Entity;
using Microsoft.EntityFrameworkCore;

namespace Aps.Infrastructure
{
    public class ApsContext : DbContext
    {
        public DbSet<ApsSemiProduct> ApsSemiProducts { get; set; }
        public DbSet<ApsManufactureProcess> ApsManufactureProcesses { get; set; }
        public DbSet<ApsOrder> ApsOrders { get; set; }
        public DbSet<ApsResource> ApsResources { get; set; }
        public DbSet<ApsAssemblyProcess> ApsAssemblyProcesses { get; set; }

        public DbSet<ApsAssemblyProcessSemiProduct> ApsAssemblyProcessSemiProducts { get; set; }
        public DbSet<ApsProcessResource> ApsProcessResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApsSemiProduct>()
                .HasMany(x => x.ApsManufactureProcesses)
                .WithOne();

            modelBuilder.Entity<ApsManufactureProcess>()
                .HasOne(x => x.PrevPart)
                .WithOne()
                .HasForeignKey<ApsManufactureProcess>(x => x.PartId);

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasKey(k => new {k.ApsAssemblyProcessId, k.ApsSemiProductId});

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasOne(x => x.ApsAssemblyProcess)
                .WithMany(x => x.InputSemiFinishedProducts)
                .HasForeignKey(x => x.ApsAssemblyProcessId);

            modelBuilder.Entity<ApsAssemblyProcessSemiProduct>()
                .HasOne(x => x.ApsSemiProduct)
                .WithMany()
                .HasForeignKey(x => x.ApsSemiProductId);

            modelBuilder.Entity<ApsProcessResource>()
                .HasKey(x => new {x.ApsProcessId, x.ResourceAttribute});


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
                    ProductId = x,
                });


            var allSemiProduct = allSemiProductString.Split(',').ToHashSet().Select(x => new ApsSemiProduct()
            {
                SemiProductId = x,
            });

            modelBuilder.Entity<ApsProduct>()
                .HasData(allProduct);

            modelBuilder.Entity<ApsSemiProduct>()
                .HasData(allSemiProduct);


            modelBuilder.Entity<ApsAssemblyProcess>()
                .HasData(new List<ApsAssemblyProcess>()
                {
                    new()
                    {
                        PartId = "process_end_A",
                        PartName = "process_end_A",
                        ProductionMode = ProductionMode.Sp,
                        MinimumProductionQuantity = 1,
                        MaximumProductionQuantity = 1,
                        Workspace = Workspace.装配,
                        ProductionTime = 4,
                    }
                });

            modelBuilder.Entity<ApsManufactureProcess>()
                .HasData(new List<ApsManufactureProcess>()
                {
                    new()
                    {
                        PartId = "process_1_a",
                        PartName = "process_1_a",
                        ProductionMode = ProductionMode.Sp,
                        MaximumProductionQuantity = 1,
                        MinimumProductionQuantity = 1,
                        Workspace = Workspace.加工,
                        ProductionTime = 1,
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


            modelBuilder.Entity<ApsProcessResource>()
                .HasData(new List<ApsProcessResource>()
                {
                    new()
                    {
                        Amount = 3,
                        ApsProcessId = "process_end_A",
                        ResourceAttribute = "机床",
                    },
                    new()
                    {
                        Amount = 2,
                        ApsProcessId = "process_end_A",
                        ResourceAttribute = "高级机床",
                    },
                    new()
                    {
                        Amount = 1,
                        ApsProcessId = "process_end_A",
                        ResourceAttribute = "人员",
                    },
                    new()
                    {
                        Amount = 1,
                        ResourceAttribute = "高级人员",
                        ApsProcessId = "process_end_A",
                    },
                    new()
                    {
                        Amount = 3,
                        ResourceAttribute = "设备",
                        ApsProcessId = "process_end_A",
                    },
                    new()
                    {
                        Amount = 2,
                        ResourceAttribute = "高级设备",
                        ApsProcessId = "process_end_A",
                    },

                    new()
                    {
                        Amount = 3,
                        ApsProcessId = "process_1_a",
                        ResourceAttribute = "机床",
                    },
                    new()
                    {
                        Amount = 2,
                        ApsProcessId = "process_1_a",
                        ResourceAttribute = "高级机床",
                    },
                    new()
                    {
                        Amount = 1,
                        ApsProcessId = "process_1_a",
                        ResourceAttribute = "人员",
                    },
                    new()
                    {
                        Amount = 1,
                        ResourceAttribute = "高级人员",
                        ApsProcessId = "process_1_a",
                    },
                    new()
                    {
                        Amount = 3,
                        ResourceAttribute = "设备",
                        ApsProcessId = "process_1_a",
                    },
                    new()
                    {
                        Amount = 2,
                        ResourceAttribute = "高级设备",
                        ApsProcessId = "process_1_a",
                    }
                });
        }

        public ApsContext(DbContextOptions<ApsContext> options) : base(options)
        {
        }

        public DbSet<Aps.Entity.ApsProduct> ApsProduct { get; set; }
    }
}