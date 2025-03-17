using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    faculty_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    faculty_code = table.Column<string>(type: "varchar(10)", nullable: false),
                    faculty_name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.faculty_id);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    program_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    program_code = table.Column<string>(type: "varchar(10)", nullable: false),
                    program_name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.program_id);
                });

            migrationBuilder.CreateTable(
                name: "StudentStatuses",
                columns: table => new
                {
                    student_status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    student_status_code = table.Column<string>(type: "varchar(10)", nullable: false),
                    student_status_name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStatuses", x => x.student_status_id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false),
                    student_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    course = table.Column<string>(type: "varchar(10)", nullable: false),
                    phone = table.Column<string>(type: "varchar(10)", nullable: false),
                    email = table.Column<string>(type: "varchar(50)", nullable: false),
                    program_id = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    faculty_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_Students_Faculties_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "Faculties",
                        principalColumn: "faculty_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Programs_program_id",
                        column: x => x.program_id,
                        principalTable: "Programs",
                        principalColumn: "program_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudentStatuses_status_id",
                        column: x => x.status_id,
                        principalTable: "StudentStatuses",
                        principalColumn: "student_status_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    address_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    permanent_address = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    temporary_address = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    mailing_address = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_Addresses_Students_student_id",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identities",
                columns: table => new
                {
                    identity_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    identity_tyoe = table.Column<int>(type: "int", nullable: false),
                    identity_number = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    issue_date = table.Column<DateTime>(type: "date", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: false),
                    country = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    has_chip = table.Column<bool>(type: "bit", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identities", x => x.identity_id);
                    table.ForeignKey(
                        name: "FK_Identities_Students_student_id",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "faculty_id", "faculty_code", "faculty_name" },
                values: new object[,]
                {
                    { 1, "CNTT", "Information Technology" },
                    { 2, "BA", "Business Administration" }
                });

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "program_id", "program_code", "program_name" },
                values: new object[,]
                {
                    { 1, "SE", "Software Engineering" },
                    { 2, "CS", "Computer Science" },
                    { 3, "BA", "Business Administration" }
                });

            migrationBuilder.InsertData(
                table: "StudentStatuses",
                columns: new[] { "student_status_id", "student_status_code", "student_status_name" },
                values: new object[,]
                {
                    { 1, "ACT", "Active" },
                    { 2, "IAC", "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_student_id",
                table: "Addresses",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_faculty_code",
                table: "Faculties",
                column: "faculty_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_faculty_name",
                table: "Faculties",
                column: "faculty_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identities_student_id",
                table: "Identities",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_program_code",
                table: "Programs",
                column: "program_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_program_name",
                table: "Programs",
                column: "program_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_email",
                table: "Students",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_faculty_id",
                table: "Students",
                column: "faculty_id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_program_id",
                table: "Students",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_status_id",
                table: "Students",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStatuses_student_status_code",
                table: "StudentStatuses",
                column: "student_status_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentStatuses_student_status_name",
                table: "StudentStatuses",
                column: "student_status_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Identities");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "StudentStatuses");
        }
    }
}
