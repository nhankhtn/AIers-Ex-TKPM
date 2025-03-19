using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UniqueFaculty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_faculty_name",
                table: "faculty",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_faculty_name",
                table: "faculty");
        }
    }
}
