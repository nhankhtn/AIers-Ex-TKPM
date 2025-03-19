using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_delete_restrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_faculties_faculty_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_programs_program_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_student_status_status_id",
                table: "students");

            migrationBuilder.AddForeignKey(
                name: "FK_students_faculties_faculty_id",
                table: "students",
                column: "faculty_id",
                principalTable: "faculties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_students_programs_program_id",
                table: "students",
                column: "program_id",
                principalTable: "programs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_students_student_status_status_id",
                table: "students",
                column: "status_id",
                principalTable: "student_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_faculties_faculty_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_programs_program_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_student_status_status_id",
                table: "students");

            migrationBuilder.AddForeignKey(
                name: "FK_students_faculties_faculty_id",
                table: "students",
                column: "faculty_id",
                principalTable: "faculties",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_programs_program_id",
                table: "students",
                column: "program_id",
                principalTable: "programs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_students_student_status_status_id",
                table: "students",
                column: "status_id",
                principalTable: "student_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
