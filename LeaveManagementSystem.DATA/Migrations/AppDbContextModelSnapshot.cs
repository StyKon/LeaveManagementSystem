﻿// <auto-generated />
using System;
using LeaveManagementSystem.DATA.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LeaveManagementSystem.DATA.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.4");

            modelBuilder.Entity("LeaveManagementSystem.DOMAINE.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("JoiningDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = "IT",
                            FullName = "Khalil Frikha",
                            JoiningDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            Department = "IT",
                            FullName = "Ahmed",
                            JoiningDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            Department = "IT",
                            FullName = "Heni",
                            JoiningDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            Department = "HR",
                            FullName = "Mohamed",
                            JoiningDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            Department = "HR",
                            FullName = "Aicha",
                            JoiningDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 6,
                            Department = "Finance",
                            FullName = "Nada",
                            JoiningDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("LeaveManagementSystem.DOMAINE.Entities.LeaveRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("LeaveType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reason")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("LeaveRequests");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = 1,
                            EndDate = new DateTime(2025, 4, 23, 0, 0, 0, 0, DateTimeKind.Local),
                            LeaveType = 0,
                            Reason = "Family needs",
                            StartDate = new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Local),
                            Status = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmployeeId = 5,
                            EndDate = new DateTime(2025, 4, 23, 0, 0, 0, 0, DateTimeKind.Local),
                            LeaveType = 0,
                            StartDate = new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Local),
                            Status = 1
                        });
                });

            modelBuilder.Entity("LeaveManagementSystem.DOMAINE.Entities.LeaveRequest", b =>
                {
                    b.HasOne("LeaveManagementSystem.DOMAINE.Entities.Employee", "Employee")
                        .WithMany("LeaveRequests")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("LeaveManagementSystem.DOMAINE.Entities.Employee", b =>
                {
                    b.Navigation("LeaveRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
