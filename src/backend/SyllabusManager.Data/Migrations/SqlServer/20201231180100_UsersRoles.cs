using Microsoft.EntityFrameworkCore.Migrations;

namespace SyllabusManager.Data.Migrations.SqlServer
{
    public partial class UsersRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcome_Specialization_SpecializationCode",
                table: "LearningOutcome");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialization_FieldsOfStudies_FieldOfStudyCode",
                table: "Specialization");

            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Specialization_SpecializationCode",
                table: "Syllabuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialization",
                table: "Specialization");

            migrationBuilder.RenameTable(
                name: "Specialization",
                newName: "Specializations");

            migrationBuilder.RenameIndex(
                name: "IX_Specialization_FieldOfStudyCode",
                table: "Specializations",
                newName: "IX_Specializations_FieldOfStudyCode");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleId1",
                table: "AspNetUserRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "AspNetUserRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                table: "AspNetUserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId1",
                table: "AspNetUserRoles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                table: "AspNetUserRoles",
                column: "RoleId1",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                table: "AspNetUserRoles",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningOutcome_Specializations_SpecializationCode",
                table: "LearningOutcome",
                column: "SpecializationCode",
                principalTable: "Specializations",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Specializations_FieldsOfStudies_FieldOfStudyCode",
                table: "Specializations",
                column: "FieldOfStudyCode",
                principalTable: "FieldsOfStudies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_Specializations_SpecializationCode",
                table: "Syllabuses",
                column: "SpecializationCode",
                principalTable: "Specializations",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcome_Specializations_SpecializationCode",
                table: "LearningOutcome");

            migrationBuilder.DropForeignKey(
                name: "FK_Specializations_FieldsOfStudies_FieldOfStudyCode",
                table: "Specializations");

            migrationBuilder.DropForeignKey(
                name: "FK_Syllabuses_Specializations_SpecializationCode",
                table: "Syllabuses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "Specializations",
                newName: "Specialization");

            migrationBuilder.RenameIndex(
                name: "IX_Specializations_FieldOfStudyCode",
                table: "Specialization",
                newName: "IX_Specialization_FieldOfStudyCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialization",
                table: "Specialization",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningOutcome_Specialization_SpecializationCode",
                table: "LearningOutcome",
                column: "SpecializationCode",
                principalTable: "Specialization",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialization_FieldsOfStudies_FieldOfStudyCode",
                table: "Specialization",
                column: "FieldOfStudyCode",
                principalTable: "FieldsOfStudies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabuses_Specialization_SpecializationCode",
                table: "Syllabuses",
                column: "SpecializationCode",
                principalTable: "Specialization",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
