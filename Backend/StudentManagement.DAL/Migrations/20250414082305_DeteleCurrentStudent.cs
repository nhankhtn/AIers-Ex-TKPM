using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeteleCurrentStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_register_cancellation_history_class_student_student_id_class_id",
                table: "register_cancellation_history");

            migrationBuilder.DropIndex(
                name: "IX_register_cancellation_history_student_id_class_id",
                table: "register_cancellation_history");

            migrationBuilder.DropPrimaryKey(
                name: "PK_class_student",
                table: "class_student");

            migrationBuilder.DropIndex(
                name: "IX_class_student_class_id",
                table: "class_student");

            migrationBuilder.DropColumn(
                name: "current_students",
                table: "classes");

            migrationBuilder.AlterColumn<string>(
                name: "student_id",
                table: "register_cancellation_history",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8)");

            migrationBuilder.AlterColumn<string>(
                name: "class_id",
                table: "register_cancellation_history",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)");

            migrationBuilder.AlterColumn<double>(
                name: "score",
                table: "class_student",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "GPA",
                table: "class_student",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RegisterCancellationHistoriesId",
                table: "class_student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "final_score",
                table: "class_student",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "grade",
                table: "class_student",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_passed",
                table: "class_student",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_class_student",
                table: "class_student",
                columns: new[] { "class_id", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_class_student_RegisterCancellationHistoriesId",
                table: "class_student",
                column: "RegisterCancellationHistoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_class_student_student_id",
                table: "class_student",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_class_student_register_cancellation_history_RegisterCancellationHistoriesId",
                table: "class_student",
                column: "RegisterCancellationHistoriesId",
                principalTable: "register_cancellation_history",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_class_student_register_cancellation_history_RegisterCancellationHistoriesId",
                table: "class_student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_class_student",
                table: "class_student");

            migrationBuilder.DropIndex(
                name: "IX_class_student_RegisterCancellationHistoriesId",
                table: "class_student");

            migrationBuilder.DropIndex(
                name: "IX_class_student_student_id",
                table: "class_student");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "class_student");

            migrationBuilder.DropColumn(
                name: "RegisterCancellationHistoriesId",
                table: "class_student");

            migrationBuilder.DropColumn(
                name: "final_score",
                table: "class_student");

            migrationBuilder.DropColumn(
                name: "grade",
                table: "class_student");

            migrationBuilder.DropColumn(
                name: "is_passed",
                table: "class_student");

            migrationBuilder.AlterColumn<string>(
                name: "student_id",
                table: "register_cancellation_history",
                type: "varchar(8)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "class_id",
                table: "register_cancellation_history",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "current_students",
                table: "classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "score",
                table: "class_student",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_class_student",
                table: "class_student",
                columns: new[] { "student_id", "class_id" });

            migrationBuilder.CreateIndex(
                name: "IX_register_cancellation_history_student_id_class_id",
                table: "register_cancellation_history",
                columns: new[] { "student_id", "class_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_class_student_class_id",
                table: "class_student",
                column: "class_id");

            migrationBuilder.AddForeignKey(
                name: "FK_register_cancellation_history_class_student_student_id_class_id",
                table: "register_cancellation_history",
                columns: new[] { "student_id", "class_id" },
                principalTable: "class_student",
                principalColumns: new[] { "student_id", "class_id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
