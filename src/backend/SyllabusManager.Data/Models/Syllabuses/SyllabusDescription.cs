using SyllabusManager.Data.Enums.Syllabuses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyllabusManager.Data.Models.Syllabuses
{
    public class SyllabusDescription : ModelBase
    {
        public int NumOfSemesters { get; set; }
        [NotMapped]
        public int Ects => NumOfSemesters * 30;
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
