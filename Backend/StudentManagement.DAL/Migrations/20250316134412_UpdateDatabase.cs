using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
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
                    gender = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    course = table.Column<string>(type: "varchar(10)", nullable: false),
                    phone = table.Column<string>(type: "varchar(10)", nullable: false),
                    email = table.Column<string>(type: "varchar(50)", nullable: false),
                    program_id = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    address_id = table.Column<int>(type: "int", nullable: false),
                    faculty_id = table.Column<int>(type: "int", nullable: false),
                    identity_id = table.Column<int>(type: "int", nullable: false)
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
                name: "Address",
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
                    table.PrimaryKey("PK_Address", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_Address_Students_student_id",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Identity",
                columns: table => new
                {
                    identity_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_Identity", x => x.identity_id);
                    table.ForeignKey(
                        name: "FK_Identity_Students_student_id",
                        column: x => x.student_id,
                        principalTable: "Students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_student_id",
                table: "Address",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Identity_student_id",
                table: "Identity",
                column: "student_id",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Identity");

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
