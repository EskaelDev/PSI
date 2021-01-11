using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SyllabusManager.Data.Migrations.SqlServer
{
    public partial class SyllabusPointLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointLimit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    ModuleType = table.Column<int>(nullable: false),
                    KindOfSubject = table.Column<int>(nullable: false),
                    SyllabusId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointLimit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointLimit_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointLimit_SyllabusId",
                table: "PointLimit",
                column: "SyllabusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointLimit");
        }
    }
}
