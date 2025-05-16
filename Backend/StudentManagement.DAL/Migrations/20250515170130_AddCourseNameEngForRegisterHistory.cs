using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseNameEngForRegisterHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "course_name_eng",
                table: "register_cancellation_history",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "course_name_eng",
                table: "register_cancellation_history");
        }
    }
}
