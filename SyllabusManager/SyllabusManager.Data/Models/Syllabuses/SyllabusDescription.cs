using SyllabusManager.Data.Enums.Syllabuses;

namespace SyllabusManager.Data.Models.Syllabuses
{
    public partial class SyllabusDescription : ModelBase
    {
        public int NumOfSemesters { get; set; }
        public int Ects { get; set; }
        public string Prerequisites { get; set; }
        public ProfessionalTitle? ProfessionalTitleAfterGraduation { get; set; }
        public string EmploymentOpportunities { get; set; }
        public string PossibilityOfContinuation { get; set; }
        public GraduationForm? FormOfGraduation { get; set; }
    }
}
