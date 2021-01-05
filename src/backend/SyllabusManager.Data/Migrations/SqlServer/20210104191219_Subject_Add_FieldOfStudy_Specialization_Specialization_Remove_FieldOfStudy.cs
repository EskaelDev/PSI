using Microsoft.EntityFrameworkCore.Migrations;

namespace SyllabusManager.Data.Migrations.SqlServer
{
    public partial class Subject_Add_FieldOfStudy_Specialization_Specialization_Remove_FieldOfStudy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FieldOfStudyCode",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecializationCode",
                table: "Subjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_FieldOfStudyCode",
                table: "Subjects",
                column: "FieldOfStudyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SpecializationCode",
                table: "Subjects",
                column: "SpecializationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_FieldsOfStudies_FieldOfStudyCode",
                table: "Subjects",
                column: "FieldOfStudyCode",
                principalTable: "FieldsOfStudies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Specializations_SpecializationCode",
                table: "Subjects",
                column: "SpecializationCode",
                principalTable: "Specializations",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_FieldsOfStudies_FieldOfStudyCode",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Specializations_SpecializationCode",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_FieldOfStudyCode",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_SpecializationCode",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyCode",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SpecializationCode",
                table: "Subjects");
        }
    }
}
