using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCourseGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_ParentId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "CourseGroups");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_ParentId",
                table: "CourseGroups",
                newName: "IX_CourseGroups_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseGroups",
                table: "CourseGroups",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseGroups_CourseGroups_ParentId",
                table: "CourseGroups",
                column: "ParentId",
                principalTable: "CourseGroups",
                principalColumn: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseGroups_CourseGroups_ParentId",
                table: "CourseGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseGroups",
                table: "CourseGroups");

            migrationBuilder.RenameTable(
                name: "CourseGroups",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_CourseGroups_ParentId",
                table: "Courses",
                newName: "IX_Courses_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_ParentId",
                table: "Courses",
                column: "ParentId",
                principalTable: "Courses",
                principalColumn: "GroupId");
        }
    }
}
