using SyllabusManager.Data.Enums.Syllabuses;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Syllabuses
{
    public partial class SyllabusDescription : ModelBase
    {
        public int NumOfSemesters { get; set; }
        public int Ects { get; set; }
        [Required]
        public string Prerequisites { get; set; }
        public ProfessionalTitle ProfessionalTitleAfterGraduation { get; set; }
        [Required]
        public string EmploymentOpportunities { get; set; }
        [Required]
        public string PossibilityOfContinuation { get; set; }
        public GraduationForm FormOfGraduation { get; set; }
    }
}
