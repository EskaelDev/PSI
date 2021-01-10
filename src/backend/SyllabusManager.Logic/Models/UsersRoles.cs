namespace SyllabusManager.Logic.Models
{
    public static class UsersRoles
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string StudentGovernment = "StudentGovernment";

        public static string[] All => new string[] { Admin, Teacher, StudentGovernment };

        public const string AdminTeacher = "Admin, Teacher";
        public const string AdminStudentGovernment = "Admin, StudentGovernment";

    }
}
