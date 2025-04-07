using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ScoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    credits = table.Column<int>(type: "int", nullable: false),
                    faculty_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    required_course_id = table.Column<int>(type: "int", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_courses_courses_required_course_id",
                        column: x => x.required_course_id,
                        principalTable: "courses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_courses_faculties_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "faculties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    academic_year = table.Column<int>(type: "int", nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false),
                    teacher_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    max_students = table.Column<int>(type: "int", nullable: false),
                    room = table.Column<string>(type: "varchar(10)", nullable: false),
                    day_of_week = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    end_time = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    deadline = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                    table.ForeignKey(
                        name: "FK_classes_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_student",
                columns: table => new
                {
                    class_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false),
                    score = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_student", x => new { x.student_id, x.class_id });
                    table.ForeignKey(
                        name: "FK_class_student_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_student_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "register_cancellation_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_register_cancellation_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_register_cancellation_history_class_student_student_id_class_id",
                        columns: x => new { x.student_id, x.class_id },
                        principalTable: "class_student",
                        principalColumns: new[] { "student_id", "class_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_student_class_id",
                table: "class_student",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_course_id",
                table: "classes",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_faculty_id",
                table: "courses",
                column: "faculty_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_name",
                table: "courses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_courses_required_course_id",
                table: "courses",
                column: "required_course_id");

            migrationBuilder.CreateIndex(
                name: "IX_register_cancellation_history_student_id_class_id",
                table: "register_cancellation_history",
                columns: new[] { "student_id", "class_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "register_cancellation_history");

            migrationBuilder.DropTable(
                name: "class_student");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "courses");
        }
    }
}
