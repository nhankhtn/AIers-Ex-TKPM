using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_db_18031046 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropIndex(
                name: "IX_status_code",
                table: "status");

            migrationBuilder.DropIndex(
                name: "IX_program_code",
                table: "program");

            migrationBuilder.DropIndex(
                name: "IX_program_name",
                table: "program");

            migrationBuilder.DropIndex(
                name: "IX_faculty_code",
                table: "faculty");

            migrationBuilder.DropIndex(
                name: "IX_faculty_name",
                table: "faculty");

            migrationBuilder.DropColumn(
                name: "code",
                table: "status");

            migrationBuilder.DropColumn(
                name: "code",
                table: "program");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "program");

            migrationBuilder.DropColumn(
                name: "updated_time",
                table: "program");

            migrationBuilder.DropColumn(
                name: "code",
                table: "faculty");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "faculty");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "faculty");

            migrationBuilder.AlterColumn<string>(
                name: "mailing_address",
                table: "student",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "student",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "Male",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true,
                oldDefaultValue: "Male");

            migrationBuilder.AddColumn<string>(
                name: "nationality",
                table: "student",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "program",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "faculty",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_name",
                table: "program",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_program_name",
                table: "program");

            migrationBuilder.DropColumn(
                name: "nationality",
                table: "student");

            migrationBuilder.AlterColumn<string>(
                name: "mailing_address",
                table: "student",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "student",
                type: "varchar(10)",
                nullable: true,
                defaultValue: "Male",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldDefaultValue: "Male");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "status",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "program",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "program",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "program",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_time",
                table: "program",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "faculty",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "faculty",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "faculty",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "faculty",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(20)", nullable: false)
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
                name: "IX_status_code",
                table: "status",
                column: "code",
                unique: true,
                filter: "[code] IS NOT NULL");

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
                name: "IX_Nationalities_student_id",
                table: "Nationalities",
                column: "student_id",
                unique: true);
        }
    }
}
