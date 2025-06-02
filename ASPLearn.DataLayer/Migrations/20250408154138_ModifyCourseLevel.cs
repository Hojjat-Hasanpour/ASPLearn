using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCourseLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_courseLevels_LevelId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courseLevels",
                table: "courseLevels");

            migrationBuilder.RenameTable(
                name: "courseLevels",
                newName: "CourseLevels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseLevels",
                table: "CourseLevels",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseLevels_LevelId",
                table: "Courses",
                column: "LevelId",
                principalTable: "CourseLevels",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseLevels_LevelId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseLevels",
                table: "CourseLevels");

            migrationBuilder.RenameTable(
                name: "CourseLevels",
                newName: "courseLevels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_courseLevels",
                table: "courseLevels",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_courseLevels_LevelId",
                table: "Courses",
                column: "LevelId",
                principalTable: "courseLevels",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
