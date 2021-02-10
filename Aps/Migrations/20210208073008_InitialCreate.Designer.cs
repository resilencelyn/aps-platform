﻿// <auto-generated />
using System;
using Aps.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aps.Migrations
{
    [DbContext(typeof(ApsContext))]
    [Migration("20210208073008_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Aps.Shared.Entity.ApsAssemblyProcessSemiProduct", b =>
                {
                    b.Property<string>("ApsAssemblyProcessId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ApsSemiProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("ApsAssemblyProcessId", "ApsSemiProductId");

                    b.HasIndex("ApsSemiProductId");

                    b.ToTable("ApsAssemblyProcessSemiProduct");

                    b.HasData(
                        new
                        {
                            ApsAssemblyProcessId = "process_end_A",
                            ApsSemiProductId = "product_semi_d",
                            Amount = 1
                        },
                        new
                        {
                            ApsAssemblyProcessId = "process_end_A",
                            ApsSemiProductId = "product_semi_o",
                            Amount = 1
                        },
                        new
                        {
                            ApsAssemblyProcessId = "process_end_A",
                            ApsSemiProductId = "product_semi_a",
                            Amount = 1
                        },
                        new
                        {
                            ApsAssemblyProcessId = "process_end_A",
                            ApsSemiProductId = "product_semi_j",
                            Amount = 1
                        },
                        new
                        {
                            ApsAssemblyProcessId = "process_end_A",
                            ApsSemiProductId = "product_semi_r",
                            Amount = 1
                        },
                        new
                        {
                            ApsAssemblyProcessId = "process_end_A",
                            ApsSemiProductId = "product_semi_f",
                            Amount = 1
                        });
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsOrder", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EarliestStartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LatestEndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("ApsOrders");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProcess", b =>
                {
                    b.Property<string>("PartId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("MaximumProductionQuantity")
                        .HasColumnType("int");

                    b.Property<int?>("MinimumProductionQuantity")
                        .HasColumnType("int");

                    b.Property<string>("PartName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ProductionMode")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("ProductionTime")
                        .HasColumnType("time(6)");

                    b.Property<int>("Workspace")
                        .HasColumnType("int");

                    b.HasKey("PartId");

                    b.ToTable("ApsProcess");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApsProcess");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProcessResource", b =>
                {
                    b.Property<string>("ApsProcessId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("ResourceClassId")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("ApsProcessId", "ResourceClassId");

                    b.HasIndex("ResourceClassId");

                    b.ToTable("ApsProcessResource");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProduct", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ApsAssemblyProcessId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("ProductId");

                    b.HasIndex("ApsAssemblyProcessId")
                        .IsUnique();

                    b.ToTable("ApsProducts");

                    b.HasData(
                        new
                        {
                            ProductId = "product_1"
                        },
                        new
                        {
                            ProductId = "product_2"
                        },
                        new
                        {
                            ProductId = "product_3"
                        },
                        new
                        {
                            ProductId = "product_4"
                        },
                        new
                        {
                            ProductId = "product_5"
                        },
                        new
                        {
                            ProductId = "product_6"
                        });
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProductSemiProduct", b =>
                {
                    b.Property<string>("ApsSemiProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ApsProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("ApsSemiProductId", "ApsProductId");

                    b.HasIndex("ApsProductId");

                    b.ToTable("ApsProductSemiProduct");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsResource", b =>
                {
                    b.Property<string>("ResourceId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int?>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("ResourceType")
                        .HasColumnType("int");

                    b.Property<int>("Workspace")
                        .HasColumnType("int");

                    b.HasKey("ResourceId");

                    b.ToTable("ApsResources");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsSemiProduct", b =>
                {
                    b.Property<string>("SemiProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("SemiProductId");

                    b.ToTable("ApsSemiProducts");

                    b.HasData(
                        new
                        {
                            SemiProductId = "product_semi_d"
                        },
                        new
                        {
                            SemiProductId = "product_semi_o"
                        },
                        new
                        {
                            SemiProductId = "product_semi_a"
                        },
                        new
                        {
                            SemiProductId = "product_semi_j"
                        },
                        new
                        {
                            SemiProductId = "product_semi_r"
                        },
                        new
                        {
                            SemiProductId = "product_semi_f"
                        },
                        new
                        {
                            SemiProductId = "product_semi_s"
                        },
                        new
                        {
                            SemiProductId = "product_semi_e"
                        },
                        new
                        {
                            SemiProductId = "product_semi_n"
                        },
                        new
                        {
                            SemiProductId = " product_semi_f"
                        },
                        new
                        {
                            SemiProductId = " product_semi_a"
                        },
                        new
                        {
                            SemiProductId = "product_semi_p"
                        },
                        new
                        {
                            SemiProductId = "product_semi_g"
                        },
                        new
                        {
                            SemiProductId = " product_semi_d"
                        },
                        new
                        {
                            SemiProductId = "product_semi_c"
                        },
                        new
                        {
                            SemiProductId = "product_semi_t"
                        },
                        new
                        {
                            SemiProductId = "product_semi_l"
                        },
                        new
                        {
                            SemiProductId = "product_semi_q"
                        },
                        new
                        {
                            SemiProductId = ""
                        });
                });

            modelBuilder.Entity("Aps.Shared.Entity.ResourceClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ResourceClassName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ResourceClass");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ResourceClassWithResource", b =>
                {
                    b.Property<int>("ResourceClassId")
                        .HasColumnType("int");

                    b.Property<string>("ApsResourceId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("IsBasic")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("ResourceClassId", "ApsResourceId");

                    b.HasIndex("ApsResourceId");

                    b.ToTable("ResourceClassWithResource");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsAssemblyProcess", b =>
                {
                    b.HasBaseType("Aps.Shared.Entity.ApsProcess");

                    b.Property<string>("OutputFinishedProductId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasDiscriminator().HasValue("ApsAssemblyProcess");

                    b.HasData(
                        new
                        {
                            PartId = "process_end_A",
                            MaximumProductionQuantity = 1,
                            MinimumProductionQuantity = 1,
                            PartName = "process_end_A",
                            ProductionMode = 2,
                            ProductionTime = new TimeSpan(0, 0, 4, 0, 0),
                            Workspace = 2
                        });
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsManufactureProcess", b =>
                {
                    b.HasBaseType("Aps.Shared.Entity.ApsProcess");

                    b.Property<string>("ApsSemiProductSemiProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("PrevPartId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasIndex("ApsSemiProductSemiProductId");

                    b.HasDiscriminator().HasValue("ApsManufactureProcess");

                    b.HasData(
                        new
                        {
                            PartId = "process_1_a",
                            MaximumProductionQuantity = 1,
                            MinimumProductionQuantity = 1,
                            PartName = "process_1_a",
                            ProductionMode = 2,
                            ProductionTime = new TimeSpan(0, 0, 0, 1, 0),
                            Workspace = 1
                        });
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsAssemblyProcessSemiProduct", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsAssemblyProcess", "ApsAssemblyProcess")
                        .WithMany("InputSemiFinishedProducts")
                        .HasForeignKey("ApsAssemblyProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aps.Shared.Entity.ApsSemiProduct", "ApsSemiProduct")
                        .WithMany()
                        .HasForeignKey("ApsSemiProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApsAssemblyProcess");

                    b.Navigation("ApsSemiProduct");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsOrder", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsProduct", "Product")
                        .WithMany("ApsOrdersBy")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProcessResource", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsProcess", "ApsProcess")
                        .WithMany("ApsResources")
                        .HasForeignKey("ApsProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aps.Shared.Entity.ResourceClass", "ResourceClass")
                        .WithMany()
                        .HasForeignKey("ResourceClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApsProcess");

                    b.Navigation("ResourceClass");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProduct", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsAssemblyProcess", "ApsAssemblyProcess")
                        .WithOne("OutputFinishedProduct")
                        .HasForeignKey("Aps.Shared.Entity.ApsProduct", "ApsAssemblyProcessId");

                    b.Navigation("ApsAssemblyProcess");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProductSemiProduct", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsProduct", "ApsProduct")
                        .WithMany("AssembleBySemiProducts")
                        .HasForeignKey("ApsProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aps.Shared.Entity.ApsSemiProduct", "ApsSemiProduct")
                        .WithMany("ApsProductsFromRequisite")
                        .HasForeignKey("ApsSemiProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApsProduct");

                    b.Navigation("ApsSemiProduct");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ResourceClassWithResource", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsResource", "ApsResource")
                        .WithMany("ResourceAttributes")
                        .HasForeignKey("ApsResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aps.Shared.Entity.ResourceClass", "ResourceClass")
                        .WithMany("ApsResources")
                        .HasForeignKey("ResourceClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApsResource");

                    b.Navigation("ResourceClass");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsManufactureProcess", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsSemiProduct", null)
                        .WithMany("ApsManufactureProcesses")
                        .HasForeignKey("ApsSemiProductSemiProductId");

                    b.HasOne("Aps.Shared.Entity.ApsManufactureProcess", "PrevPart")
                        .WithOne()
                        .HasForeignKey("Aps.Shared.Entity.ApsManufactureProcess", "PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrevPart");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProcess", b =>
                {
                    b.Navigation("ApsResources");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProduct", b =>
                {
                    b.Navigation("ApsOrdersBy");

                    b.Navigation("AssembleBySemiProducts");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsResource", b =>
                {
                    b.Navigation("ResourceAttributes");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsSemiProduct", b =>
                {
                    b.Navigation("ApsManufactureProcesses");

                    b.Navigation("ApsProductsFromRequisite");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ResourceClass", b =>
                {
                    b.Navigation("ApsResources");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsAssemblyProcess", b =>
                {
                    b.Navigation("InputSemiFinishedProducts");

                    b.Navigation("OutputFinishedProduct")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}