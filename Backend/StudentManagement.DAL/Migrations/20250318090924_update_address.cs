using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class update_address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_identity_documents_student_student_id",
                table: "identity_documents");

            migrationBuilder.DropForeignKey(
                name: "FK_student_faculties_faculty_id",
                table: "student");

            migrationBuilder.DropForeignKey(
                name: "FK_student_program_program_id",
                table: "student");

            migrationBuilder.DropForeignKey(
                name: "FK_student_student_status_status_id",
                table: "student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student",
                table: "student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_program",
                table: "program");

            migrationBuilder.RenameTable(
                name: "student",
                newName: "students");

            migrationBuilder.RenameTable(
                name: "program",
                newName: "programs");

            migrationBuilder.RenameIndex(
                name: "IX_student_status_id",
                table: "students",
                newName: "IX_students_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_student_program_id",
                table: "students",
                newName: "IX_students_program_id");

            migrationBuilder.RenameIndex(
                name: "IX_student_faculty_id",
                table: "students",
                newName: "IX_students_faculty_id");

            migrationBuilder.RenameIndex(
                name: "IX_student_email",
                table: "students",
                newName: "IX_students_email");

            migrationBuilder.RenameIndex(
                name: "IX_program_name",
                table: "programs",
                newName: "IX_programs_name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_students",
                table: "students",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_programs",
                table: "programs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_identity_documents_students_student_id",
                table: "identity_documents",
                column: "student_id",
                principalTable: "students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_identity_documents_students_student_id",
                table: "identity_documents");

            migrationBuilder.DropForeignKey(
                name: "FK_students_faculties_faculty_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_programs_program_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_students_student_status_status_id",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_students",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_programs",
                table: "programs");

            migrationBuilder.RenameTable(
                name: "students",
                newName: "student");

            migrationBuilder.RenameTable(
                name: "programs",
                newName: "program");

            migrationBuilder.RenameIndex(
                name: "IX_students_status_id",
                table: "student",
                newName: "IX_student_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_students_program_id",
                table: "student",
                newName: "IX_student_program_id");

            migrationBuilder.RenameIndex(
                name: "IX_students_faculty_id",
                table: "student",
                newName: "IX_student_faculty_id");

            migrationBuilder.RenameIndex(
                name: "IX_students_email",
                table: "student",
                newName: "IX_student_email");

            migrationBuilder.RenameIndex(
                name: "IX_programs_name",
                table: "program",
                newName: "IX_program_name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_student",
                table: "student",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_program",
                table: "program",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_identity_documents_student_student_id",
                table: "identity_documents",
                column: "student_id",
                principalTable: "student",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_faculties_faculty_id",
                table: "student",
                column: "faculty_id",
                principalTable: "faculties",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_program_program_id",
                table: "student",
                column: "program_id",
                principalTable: "program",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_student_status_status_id",
                table: "student",
                column: "status_id",
                principalTable: "student_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
