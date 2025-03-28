using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "student_statuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "order",
                value: 1);

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"),
                column: "order",
                value: 2);

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"),
                column: "order",
                value: 1);

            migrationBuilder.UpdateData(
                table: "student_statuses",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"),
                column: "order",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                table: "student_statuses");
        }
    }
}
