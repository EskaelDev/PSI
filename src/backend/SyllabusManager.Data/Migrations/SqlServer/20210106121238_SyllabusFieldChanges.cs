using Microsoft.EntityFrameworkCore.Migrations;

namespace SyllabusManager.Data.Migrations.SqlServer
{
    public partial class SyllabusFieldChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeanName",
                table: "Syllabuses");

            migrationBuilder.DropColumn(
                name: "Ects",
                table: "SyllabusDescription");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Syllabuses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThesisCourse",
                table: "Syllabuses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Syllabuses");

            migrationBuilder.DropColumn(
                name: "ThesisCourse",
                table: "Syllabuses");

            migrationBuilder.AddColumn<string>(
                name: "DeanName",
                table: "Syllabuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ects",
                table: "SyllabusDescription",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
