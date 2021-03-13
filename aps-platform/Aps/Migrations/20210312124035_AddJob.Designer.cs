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
    [Migration("20210312124035_AddJob")]
    partial class AddJob
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
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApsOrderId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ApsProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time(6)");

                    b.Property<DateTime?>("End")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("ProductInstanceId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("Start")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Workspace")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApsOrderId");

                    b.HasIndex("ApsProductId");

                    b.HasIndex("ProductInstanceId");

                    b.ToTable("ApsJob");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApsJob");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsOrder", b =>
                {
                    b.Property<string>("Id")
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
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ApsOrders");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProcess", b =>
                {
                    b.Property<string>("Id")
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

                    b.HasKey("Id");

                    b.ToTable("ApsProcess");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApsProcess");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProcessResource", b =>
                {
                    b.Property<string>("ProcessId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("ResourceClassId")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("ProcessId", "ResourceClassId");

                    b.HasIndex("ResourceClassId");

                    b.ToTable("ApsProcessResource");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProduct", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ApsAssemblyProcessId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ApsAssemblyProcessId")
                        .IsUnique();

                    b.ToTable("ApsProducts");
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
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int?>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Workspace")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ApsResources");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsSemiProduct", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ApsSemiProducts");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ProductInstance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApsProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("OrderedById")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ApsProductId");

                    b.HasIndex("OrderedById");

                    b.ToTable("ProductInstances");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ResourceClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ResourceClassName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ResourceClasses");
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

            modelBuilder.Entity("Aps.Shared.Entity.SemiProductInstance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApsSemiProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<Guid?>("ProductAssemblyToId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ApsSemiProductId");

                    b.HasIndex("ProductAssemblyToId");

                    b.ToTable("SemiProductInstances");
                });

            modelBuilder.Entity("ApsJobApsResource", b =>
                {
                    b.Property<string>("ApsResourceId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<Guid>("WorkJobsId")
                        .HasColumnType("char(36)");

                    b.HasKey("ApsResourceId", "WorkJobsId");

                    b.HasIndex("WorkJobsId");

                    b.ToTable("ApsJobApsResource");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsAssemblyJob", b =>
                {
                    b.HasBaseType("Aps.Shared.Entity.ApsJob");

                    b.Property<string>("ApsAssemblyProcessId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasIndex("ApsAssemblyProcessId");

                    b.HasDiscriminator().HasValue("ApsAssemblyJob");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsManufactureJob", b =>
                {
                    b.HasBaseType("Aps.Shared.Entity.ApsJob");

                    b.Property<string>("ApsManufactureProcessId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ApsSemiProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<Guid?>("SemiProductInstanceId")
                        .HasColumnType("char(36)");

                    b.HasIndex("ApsManufactureProcessId");

                    b.HasIndex("ApsSemiProductId");

                    b.HasIndex("SemiProductInstanceId");

                    b.HasDiscriminator().HasValue("ApsManufactureJob");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsAssemblyProcess", b =>
                {
                    b.HasBaseType("Aps.Shared.Entity.ApsProcess");

                    b.Property<string>("OutputFinishedProductId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasDiscriminator().HasValue("ApsAssemblyProcess");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsManufactureProcess", b =>
                {
                    b.HasBaseType("Aps.Shared.Entity.ApsProcess");

                    b.Property<string>("ApsSemiProductId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("PrevPartId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasIndex("ApsSemiProductId");

                    b.HasDiscriminator().HasValue("ApsManufactureProcess");
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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApsAssemblyProcess");

                    b.Navigation("ApsSemiProduct");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsJob", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsOrder", "ApsOrder")
                        .WithMany()
                        .HasForeignKey("ApsOrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Aps.Shared.Entity.ApsProduct", "ApsProduct")
                        .WithMany()
                        .HasForeignKey("ApsProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Aps.Shared.Entity.ProductInstance", "ProductInstance")
                        .WithMany()
                        .HasForeignKey("ProductInstanceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ApsOrder");

                    b.Navigation("ApsProduct");

                    b.Navigation("ProductInstance");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsOrder", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsProduct", "Product")
                        .WithMany("ApsOrdersBy")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProcessResource", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsProcess", "ApsProcess")
                        .WithMany("ApsResources")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aps.Shared.Entity.ResourceClass", "ResourceClass")
                        .WithMany()
                        .HasForeignKey("ResourceClassId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApsProcess");

                    b.Navigation("ResourceClass");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsProduct", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsAssemblyProcess", "ApsAssemblyProcess")
                        .WithOne("OutputFinishedProduct")
                        .HasForeignKey("Aps.Shared.Entity.ApsProduct", "ApsAssemblyProcessId")
                        .OnDelete(DeleteBehavior.Restrict);

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
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ApsProduct");

                    b.Navigation("ApsSemiProduct");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ProductInstance", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsProduct", "ApsProduct")
                        .WithMany()
                        .HasForeignKey("ApsProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Aps.Shared.Entity.ApsOrder", "OrderedBy")
                        .WithMany()
                        .HasForeignKey("OrderedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ApsProduct");

                    b.Navigation("OrderedBy");
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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApsResource");

                    b.Navigation("ResourceClass");
                });

            modelBuilder.Entity("Aps.Shared.Entity.SemiProductInstance", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsSemiProduct", "ApsSemiProduct")
                        .WithMany()
                        .HasForeignKey("ApsSemiProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Aps.Shared.Entity.ProductInstance", "ProductAssemblyTo")
                        .WithMany()
                        .HasForeignKey("ProductAssemblyToId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ApsSemiProduct");

                    b.Navigation("ProductAssemblyTo");
                });

            modelBuilder.Entity("ApsJobApsResource", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsResource", null)
                        .WithMany()
                        .HasForeignKey("ApsResourceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Aps.Shared.Entity.ApsJob", null)
                        .WithMany()
                        .HasForeignKey("WorkJobsId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsAssemblyJob", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsAssemblyProcess", "ApsAssemblyProcess")
                        .WithMany()
                        .HasForeignKey("ApsAssemblyProcessId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ApsAssemblyProcess");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsManufactureJob", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsManufactureProcess", "ApsManufactureProcess")
                        .WithMany()
                        .HasForeignKey("ApsManufactureProcessId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Aps.Shared.Entity.ApsSemiProduct", "ApsSemiProduct")
                        .WithMany()
                        .HasForeignKey("ApsSemiProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Aps.Shared.Entity.SemiProductInstance", "SemiProductInstance")
                        .WithMany()
                        .HasForeignKey("SemiProductInstanceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ApsManufactureProcess");

                    b.Navigation("ApsSemiProduct");

                    b.Navigation("SemiProductInstance");
                });

            modelBuilder.Entity("Aps.Shared.Entity.ApsManufactureProcess", b =>
                {
                    b.HasOne("Aps.Shared.Entity.ApsSemiProduct", null)
                        .WithMany("ApsManufactureProcesses")
                        .HasForeignKey("ApsSemiProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Aps.Shared.Entity.ApsManufactureProcess", "PrevPart")
                        .WithOne()
                        .HasForeignKey("Aps.Shared.Entity.ApsManufactureProcess", "Id")
                        .OnDelete(DeleteBehavior.Restrict)
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
