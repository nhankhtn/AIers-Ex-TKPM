using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "faculty",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faculty", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "programs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "student_status",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(8)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "Male"),
                    email = table.Column<string>(type: "varchar(50)", nullable: false),
                    course = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<string>(type: "varchar(10)", nullable: false),
                    permanent_address = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    temporary_address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    mailing_address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    program_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    faculty_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                    table.ForeignKey(
                        name: "FK_students_faculty_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "faculty",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_students_programs_program_id",
                        column: x => x.program_id,
                        principalTable: "programs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_students_student_status_status_id",
                        column: x => x.status_id,
                        principalTable: "student_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "identity_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "CCCD"),
                    document_number = table.Column<string>(type: "varchar(20)", nullable: false),
                    issue_date = table.Column<DateTime>(type: "date", nullable: false),
                    issue_place = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: false),
                    country = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    is_chip = table.Column<bool>(type: "bit", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_identity_documents_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_identity_documents_student_id",
                table: "identity_documents",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_programs_name",
                table: "programs",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_status_name",
                table: "student_status",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_email",
                table: "students",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_faculty_id",
                table: "students",
                column: "faculty_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_program_id",
                table: "students",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_status_id",
                table: "students",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identity_documents");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "faculty");

            migrationBuilder.DropTable(
                name: "programs");

            migrationBuilder.DropTable(
                name: "student_status");
        }
    }
}
