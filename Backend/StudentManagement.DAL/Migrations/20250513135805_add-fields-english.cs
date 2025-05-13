using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addfieldsenglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name_eng",
                table: "student_statuses",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name_eng",
                table: "programs",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name_eng",
                table: "faculties",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description_eng",
                table: "courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name_eng",
                table: "courses",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "faculties",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "faculties",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "faculties",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "faculties",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "programs",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "programs",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "programs",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                column: "name_eng",
                value: "");

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                column: "name_eng",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name_eng",
                table: "student_statuses");

            migrationBuilder.DropColumn(
                name: "name_eng",
                table: "programs");

            migrationBuilder.DropColumn(
                name: "name_eng",
                table: "faculties");

            migrationBuilder.DropColumn(
                name: "description_eng",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "name_eng",
                table: "courses");
        }
    }
}
