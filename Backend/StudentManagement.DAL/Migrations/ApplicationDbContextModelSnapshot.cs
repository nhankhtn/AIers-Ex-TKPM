﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.DAL.Data;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentManagement.Domain.Models.Faculty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("faculty");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Identity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("country");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("document_number");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("date")
                        .HasColumnName("expiry_date");

                    b.Property<bool>("IsChip")
                        .HasColumnType("bit")
                        .HasColumnName("is_chip");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("date")
                        .HasColumnName("issue_date");

                    b.Property<string>("IssuePlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("issue_place");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("notes");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("student_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(10)")
                        .HasDefaultValue("CCCD")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("identity_documents");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Program", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("programs");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Student", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(8)")
                        .HasColumnName("id");

                    b.Property<int>("Course")
                        .HasColumnType("int")
                        .HasColumnName("course");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("faculty_id");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(10)")
                        .HasDefaultValue("Male")
                        .HasColumnName("gender");

                    b.Property<string>("MailingAddress")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("mailing_address");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("nationality");

                    b.Property<string>("PermanentAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("permanent_address");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("phone");

                    b.Property<Guid>("ProgramId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("program_id");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("status_id");

                    b.Property<string>("TemporaryAddress")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("temporary_address");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("FacultyId");

                    b.HasIndex("ProgramId");

                    b.HasIndex("StatusId");

                    b.ToTable("students");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.StudentStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("student_status");
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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagement.Domain.Models.Program", "Program")
                        .WithMany("Students")
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagement.Domain.Models.StudentStatus", "Status")
                        .WithMany("Students")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Restrict)
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
                    b.Navigation("Identity")
                        .IsRequired();
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.StudentStatus", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
