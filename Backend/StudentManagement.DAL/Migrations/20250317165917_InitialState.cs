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
                    code = table.Column<string>(type: "varchar(10)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    created_at = table.Column<DateTime>(type: "date", nullable: true),
                    updated_at = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faculty", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "program",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "varchar(10)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    created_at = table.Column<DateTime>(type: "date", nullable: false),
                    updated_time = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    code = table.Column<string>(type: "varchar(10)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(8)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "varchar(10)", nullable: true, defaultValue: "Male"),
                    email = table.Column<string>(type: "varchar(50)", nullable: true),
                    course = table.Column<int>(type: "int", nullable: true),
                    phone = table.Column<string>(type: "varchar(10)", nullable: false),
                    temporary_address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    mailing_address = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    program_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    faculty_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_faculty_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "faculty",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_program_program_id",
                        column: x => x.program_id,
                        principalTable: "program",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ward = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    district = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    street = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                    table.ForeignKey(
                        name: "FK_address_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    is_chip = table.Column<bool>(type: "bit", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_identity_documents_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.id);
                    table.ForeignKey(
                        name: "FK_Nationalities_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_student_id",
                table: "address",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_faculty_code",
                table: "faculty",
                column: "code",
                unique: true,
                filter: "[code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_faculty_name",
                table: "faculty",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_identity_documents_student_id",
                table: "identity_documents",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nationalities_student_id",
                table: "Nationalities",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_code",
                table: "program",
                column: "code",
                unique: true,
                filter: "[code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_program_name",
                table: "program",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_status_code",
                table: "status",
                column: "code",
                unique: true,
                filter: "[code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_status_name",
                table: "status",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_email",
                table: "student",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_student_faculty_id",
                table: "student",
                column: "faculty_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_program_id",
                table: "student",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_status_id",
                table: "student",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "identity_documents");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "faculty");

            migrationBuilder.DropTable(
                name: "program");

            migrationBuilder.DropTable(
                name: "status");
        }
    }
}
