using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    meta_data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_time_utc = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    end_time_utc = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    is_success = table.Column<bool>(type: "bit", nullable: false),
                    error_message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "faculties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faculties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "programs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "register_cancellation_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    course_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    student_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    student_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false),
                    academic_year = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_register_cancellation_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email_domain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email_pattern = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "student_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    credits = table.Column<int>(type: "int", nullable: false),
                    faculty_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    required_course_id = table.Column<string>(type: "varchar(10)", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                name: "students",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(8)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "Male"),
                    email = table.Column<string>(type: "varchar(50)", nullable: false),
                    course = table.Column<int>(type: "int", nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    permanent_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    temporary_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mailing_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    program_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    faculty_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                    table.ForeignKey(
                        name: "FK_students_faculties_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "faculties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_students_programs_program_id",
                        column: x => x.program_id,
                        principalTable: "programs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_students_student_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "student_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(10)", nullable: false),
                    academic_year = table.Column<int>(type: "int", nullable: false),
                    course_id = table.Column<string>(type: "varchar(10)", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false),
                    teacher_name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    max_students = table.Column<int>(type: "int", nullable: false),
                    room = table.Column<string>(type: "varchar(10)", nullable: false),
                    day_of_week = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    end_time = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                    table.ForeignKey(
                        name: "FK_classes_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "identity_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "CCCD"),
                    number = table.Column<string>(type: "varchar(20)", nullable: false),
                    issued_date = table.Column<DateTime>(type: "date", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: false),
                    issue_place = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    is_chip = table.Column<bool>(type: "bit", nullable: false),
                    note = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_identity_documents_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_student",
                columns: table => new
                {
                    class_id = table.Column<string>(type: "varchar(10)", nullable: false),
                    student_id = table.Column<string>(type: "varchar(8)", nullable: false),
                    score = table.Column<double>(type: "float", nullable: false),
                    final_score = table.Column<double>(type: "float", nullable: false),
                    GPA = table.Column<double>(type: "float", nullable: false),
                    grade = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    is_passed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_student", x => new { x.class_id, x.student_id });
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
                name: "ClassStudent",
                columns: table => new
                {
                    ClassesClassId = table.Column<string>(type: "varchar(10)", nullable: false),
                    StudentsId = table.Column<string>(type: "varchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudent", x => new { x.ClassesClassId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_ClassStudent_classes_ClassesClassId",
                        column: x => x.ClassesClassId,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStudent_students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "email_domain", "email_pattern" },
                values: new object[] { 1, "@gmail.com", "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$" });

            migrationBuilder.InsertData(
                table: "faculties",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Khoa Toán" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Khoa Công nghệ thông tin" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "Khoa Hoá" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "Khoa Lí" }
                });

            migrationBuilder.InsertData(
                table: "programs",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Đại trà" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Chất lượng cao" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "Tiên tiến" }
                });

            migrationBuilder.InsertData(
                table: "student_statuses",
                columns: new[] { "id", "name", "order" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Đang học", 1 },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Đã tốt nghiệp", 2 },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "Đã bảo lưu", 1 },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "Đã nghỉ học", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_student_student_id",
                table: "class_student",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_course_id",
                table: "classes",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudent_StudentsId",
                table: "ClassStudent",
                column: "StudentsId");

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
                name: "IX_faculties_name",
                table: "faculties",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_documents_number",
                table: "identity_documents",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_documents_student_id",
                table: "identity_documents",
                column: "student_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_programs_name",
                table: "programs",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_statuses_name",
                table: "student_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_email",
                table: "students",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_faculty_id",
                table: "students",
                column: "faculty_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_phone",
                table: "students",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_program_id",
                table: "students",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_status_id",
                table: "students",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_entries");

            migrationBuilder.DropTable(
                name: "class_student");

            migrationBuilder.DropTable(
                name: "ClassStudent");

            migrationBuilder.DropTable(
                name: "identity_documents");

            migrationBuilder.DropTable(
                name: "register_cancellation_history");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "programs");

            migrationBuilder.DropTable(
                name: "student_statuses");

            migrationBuilder.DropTable(
                name: "faculties");
        }
    }
}
