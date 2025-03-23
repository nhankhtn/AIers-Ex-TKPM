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
                name: "audit_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    meta_data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_time_utc = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    end_time_utc = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    is_success = table.Column<bool>(type: "bit", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "faculties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faculties", x => x.id);
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
                name: "student_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(8)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "Male"),
                    email = table.Column<string>(type: "varchar(50)", nullable: false),
                    course = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<string>(type: "varchar(10)", nullable: false),
                    permanent_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    temporary_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mailing_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    program_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    faculty_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                    table.ForeignKey(
                        name: "FK_students_faculties_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "faculties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_students_programs_program_id",
                        column: x => x.program_id,
                        principalTable: "programs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_students_student_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "student_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "identity_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "CCCD"),
                    number = table.Column<string>(type: "varchar(20)", nullable: false),
                    issued_date = table.Column<DateOnly>(type: "date", nullable: false),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: false),
                    issue_place = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    is_chip = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(100)", nullable: true),
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
                name: "IX_faculties_name",
                table: "faculties",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_documents_number",
                table: "identity_documents",
                column: "number",
                unique: true);

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
                name: "IX_student_statuses_name",
                table: "student_statuses",
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
                name: "IX_students_phone",
                table: "students",
                column: "phone",
                unique: true);

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
                name: "audit_entries");

            migrationBuilder.DropTable(
                name: "identity_documents");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "faculties");

            migrationBuilder.DropTable(
                name: "programs");

            migrationBuilder.DropTable(
                name: "student_statuses");
        }
    }
}
