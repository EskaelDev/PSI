using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SyllabusManager.Data.Migrations.SqlServer
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyllabusDescription",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NumOfSemesters = table.Column<int>(nullable: false),
                    Ects = table.Column<int>(nullable: false),
                    Prerequisites = table.Column<string>(nullable: false),
                    ProfessionalTitleAfterGraduation = table.Column<int>(nullable: false),
                    EmploymentOpportunities = table.Column<string>(nullable: false),
                    PossibilityOfContinuation = table.Column<string>(nullable: false),
                    FormOfGraduation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyllabusDescription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldsOfStudies",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Profile = table.Column<int>(nullable: false),
                    BranchOfScience = table.Column<string>(nullable: true),
                    Discipline = table.Column<string>(nullable: true),
                    Faculty = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    SupervisorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsOfStudies", x => x.Code);
                    table.ForeignKey(
                        name: "FK_FieldsOfStudies_AspNetUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AcademicYear = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    NamePl = table.Column<string>(nullable: false),
                    NameEng = table.Column<string>(nullable: true),
                    ModuleType = table.Column<int>(nullable: false),
                    KindOfSubject = table.Column<int>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    TypeOfSubject = table.Column<int>(nullable: false),
                    SupervisorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_AspNetUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcomeDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AcademicYear = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FieldOfStudyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcomeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningOutcomeDocuments_FieldsOfStudies_FieldOfStudyCode",
                        column: x => x.FieldOfStudyCode,
                        principalTable: "FieldsOfStudies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    FieldOfStudyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Specializations_FieldsOfStudies_FieldOfStudyCode",
                        column: x => x.FieldOfStudyCode,
                        principalTable: "FieldsOfStudies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardEntries_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcomeEvaluation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GradingSystem = table.Column<int>(nullable: false),
                    LearningOutcomeSymbol = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SubjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcomeEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningOutcomeEvaluation_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LessonType = table.Column<int>(nullable: false),
                    HoursAtUniversity = table.Column<int>(nullable: false),
                    StudentWorkloadHours = table.Column<int>(nullable: false),
                    FormOfCrediting = table.Column<int>(nullable: false),
                    Ects = table.Column<int>(nullable: false),
                    EctsinclPracticalClasses = table.Column<int>(nullable: false),
                    EctsinclDirectTeacherStudentContactClasses = table.Column<int>(nullable: false),
                    IsFinal = table.Column<bool>(nullable: false),
                    IsScientific = table.Column<bool>(nullable: false),
                    IsGroup = table.Column<bool>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lesson_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Literature",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Authors = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Distributor = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    IsPrimary = table.Column<bool>(nullable: false),
                    Isbn = table.Column<string>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Literature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Literature_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTeacher",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(nullable: false),
                    TeacherId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeacher", x => new { x.SubjectId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeacher_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningOutcome",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Symbol = table.Column<string>(nullable: false),
                    Category = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    U1degreeCharacteristics = table.Column<string>(nullable: false),
                    S2degreePrk = table.Column<string>(nullable: true),
                    S2degreePrkeng = table.Column<string>(nullable: true),
                    SpecializationCode = table.Column<string>(nullable: true),
                    LearningOutcomeDocumentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningOutcome", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningOutcome_LearningOutcomeDocuments_LearningOutcomeDocumentId",
                        column: x => x.LearningOutcomeDocumentId,
                        principalTable: "LearningOutcomeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningOutcome_Specializations_SpecializationCode",
                        column: x => x.SpecializationCode,
                        principalTable: "Specializations",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Syllabuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AcademicYear = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StudentGovernmentOpinion = table.Column<int>(nullable: true),
                    OpinionDeadline = table.Column<DateTime>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ApprovalDate = table.Column<DateTime>(nullable: true),
                    ValidFrom = table.Column<DateTime>(nullable: true),
                    StudentRepresentativeName = table.Column<string>(nullable: true),
                    DeanName = table.Column<string>(nullable: true),
                    AuthorName = table.Column<string>(nullable: false),
                    ScopeOfDiplomaExam = table.Column<string>(nullable: false),
                    IntershipType = table.Column<string>(nullable: true),
                    DescriptionId = table.Column<Guid>(nullable: true),
                    FieldOfStudyCode = table.Column<string>(nullable: true),
                    SpecializationCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllabuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Syllabuses_SyllabusDescription_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "SyllabusDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Syllabuses_FieldsOfStudies_FieldOfStudyCode",
                        column: x => x.FieldOfStudyCode,
                        principalTable: "FieldsOfStudies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Syllabuses_Specializations_SpecializationCode",
                        column: x => x.SpecializationCode,
                        principalTable: "Specializations",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CardEntriesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardEntry_CardEntries_CardEntriesId",
                        column: x => x.CardEntriesId,
                        principalTable: "CardEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassForm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Hours = table.Column<int>(nullable: false),
                    LessonId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassForm_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectInSyllabusDescription",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AssignedSemester = table.Column<int>(nullable: false),
                    CompletionSemester = table.Column<int>(nullable: true),
                    SubjectId = table.Column<Guid>(nullable: true),
                    SyllabusId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectInSyllabusDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectInSyllabusDescription_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectInSyllabusDescription_Syllabuses_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CardEntries_SubjectId",
                table: "CardEntries",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CardEntry_CardEntriesId",
                table: "CardEntry",
                column: "CardEntriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassForm_LessonId",
                table: "ClassForm",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldsOfStudies_SupervisorId",
                table: "FieldsOfStudies",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcome_LearningOutcomeDocumentId",
                table: "LearningOutcome",
                column: "LearningOutcomeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcome_SpecializationCode",
                table: "LearningOutcome",
                column: "SpecializationCode");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomeDocuments_FieldOfStudyCode",
                table: "LearningOutcomeDocuments",
                column: "FieldOfStudyCode");

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomeEvaluation_SubjectId",
                table: "LearningOutcomeEvaluation",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_SubjectId",
                table: "Lesson",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Literature_SubjectId",
                table: "Literature",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_FieldOfStudyCode",
                table: "Specializations",
                column: "FieldOfStudyCode");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectInSyllabusDescription_SubjectId",
                table: "SubjectInSyllabusDescription",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectInSyllabusDescription_SyllabusId",
                table: "SubjectInSyllabusDescription",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SupervisorId",
                table: "Subjects",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacher_TeacherId",
                table: "SubjectTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_DescriptionId",
                table: "Syllabuses",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_FieldOfStudyCode",
                table: "Syllabuses",
                column: "FieldOfStudyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabuses_SpecializationCode",
                table: "Syllabuses",
                column: "SpecializationCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CardEntry");

            migrationBuilder.DropTable(
                name: "ClassForm");

            migrationBuilder.DropTable(
                name: "LearningOutcome");

            migrationBuilder.DropTable(
                name: "LearningOutcomeEvaluation");

            migrationBuilder.DropTable(
                name: "Literature");

            migrationBuilder.DropTable(
                name: "SubjectInSyllabusDescription");

            migrationBuilder.DropTable(
                name: "SubjectTeacher");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CardEntries");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "LearningOutcomeDocuments");

            migrationBuilder.DropTable(
                name: "Syllabuses");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "SyllabusDescription");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "FieldsOfStudies");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
