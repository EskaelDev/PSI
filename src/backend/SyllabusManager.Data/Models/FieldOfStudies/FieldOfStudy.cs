using SyllabusManager.Data.Enums.FieldOfStudies;
using SyllabusManager.Data.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.FieldOfStudies
{
    /// <summary>
    /// Kierunek studiów
    /// </summary>
    public class FieldOfStudy : NonVersionedModelBase
    {

        [Required]
        public string Name { get; set; }
        public DegreeLevel Level { get; set; }
        public StudiesProfile Profile { get; set; }
        public string BranchOfScience { get; set; }
        public string Discipline { get; set; }
        [Required] 
        public string Faculty { get; set; } = "Informatyka i Zarządzanie";
        public CourseType Type { get; set; }
        public MainLanguage Language { get; set; }
        public SyllabusManagerUser Supervisor { get; set; }
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();
    }
}