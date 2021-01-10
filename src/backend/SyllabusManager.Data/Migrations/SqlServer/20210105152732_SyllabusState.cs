using Microsoft.EntityFrameworkCore.Migrations;

namespace SyllabusManager.Data.Migrations.SqlServer
{
    public partial class SyllabusState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Syllabuses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Syllabuses");
        }
    }
}
