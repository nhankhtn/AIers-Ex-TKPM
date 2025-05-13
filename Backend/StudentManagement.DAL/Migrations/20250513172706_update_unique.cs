using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_student_statuses_name_eng",
                table: "student_statuses",
                column: "name_eng",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_programs_name_eng",
                table: "programs",
                column: "name_eng",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_faculties_name_eng",
                table: "faculties",
                column: "name_eng",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_name_eng",
                table: "courses",
                column: "name_eng",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_student_statuses_name_eng",
                table: "student_statuses");

            migrationBuilder.DropIndex(
                name: "IX_programs_name_eng",
                table: "programs");

            migrationBuilder.DropIndex(
                name: "IX_faculties_name_eng",
                table: "faculties");

            migrationBuilder.DropIndex(
                name: "IX_courses_name_eng",
                table: "courses");
        }
    }
}
