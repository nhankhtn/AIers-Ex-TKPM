﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.DAL.Data;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250316134412_UpdateDatabase")]
    partial class UpdateDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentManagement.Domain.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("address_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MailingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("mailing_address");

                    b.Property<string>("PermanentAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("permanent_address");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("student_id");

                    b.Property<string>("TemporaryAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("temporary_address");

                    b.HasKey("Id");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("Address");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("faculty_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("faculty_name");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Identity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("identity_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("country");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("date")
                        .HasColumnName("expiry_date");

                    b.Property<bool>("HasChip")
                        .HasColumnType("bit")
                        .HasColumnName("has_chip");

                    b.Property<string>("IdentityType")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("identity_number");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("date")
                        .HasColumnName("issue_date");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("notes");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("student_id");

                    b.HasKey("Id");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("Identity");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Program", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("program_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("program_name");

                    b.HasKey("Id");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Student", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(8)")
                        .HasColumnName("student_id");

                    b.Property<int>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("address_id");

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("course");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<int>("FacultyId")
                        .HasColumnType("int")
                        .HasColumnName("faculty_id");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("gender");

                    b.Property<int>("IdentityId")
                        .HasColumnType("int")
                        .HasColumnName("identity_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("student_name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("phone");

                    b.Property<int>("ProgramId")
                        .HasColumnType("int")
                        .HasColumnName("program_id");

                    b.Property<int>("StatusId")
                        .HasColumnType("int")
                        .HasColumnName("status_id");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("ProgramId");

                    b.HasIndex("StatusId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.StudentStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("student_status_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("student_status_name");

                    b.HasKey("Id");

                    b.ToTable("StudentStatuses");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Address", b =>
                {
                    b.HasOne("StudentManagement.Domain.Models.Student", "Student")
                        .WithOne("Address")
                        .HasForeignKey("StudentManagement.Domain.Models.Address", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Identity", b =>
                {
                    b.HasOne("StudentManagement.Domain.Models.Student", "Student")
                        .WithOne("Identity")
                        .HasForeignKey("StudentManagement.Domain.Models.Identity", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Student", b =>
                {
                    b.HasOne("StudentManagement.Domain.Models.Faculty", "Faculty")
                        .WithMany("Students")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagement.Domain.Models.Program", "Program")
                        .WithMany("Students")
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagement.Domain.Models.StudentStatus", "Status")
                        .WithMany("Students")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");

                    b.Navigation("Program");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Faculty", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Program", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Student", b =>
                {
                    b.Navigation("Address");

                    b.Navigation("Identity");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.StudentStatus", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
