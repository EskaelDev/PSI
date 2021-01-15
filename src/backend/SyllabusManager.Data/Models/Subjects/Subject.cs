using SyllabusManager.Data.Enums.FieldOfStudies;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.ManyToMany;
using SyllabusManager.Data.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SyllabusManager.Data.Models.Subjects
{
    public class Subject : DocumentInAcademicYearBase
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string NamePl { get; set; }
        public string NameEng { get; set; } = "";
        public ModuleType ModuleType { get; set; }
        public KindOfSubject KindOfSubject { get; set; }
        public MainLanguage Language { get; set; }
        public TypeOfSubject TypeOfSubject { get; set; }
        public SyllabusManagerUser Supervisor { get; set; }
        public List<Literature> Literature { get; set; } = new List<Literature>();
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        public List<LearningOutcomeEvaluation> LearningOutcomeEvaluations { get; set; } = new List<LearningOutcomeEvaluation>();
        public List<CardEntries> CardEntries { get; set; } = new List<CardEntries>();
        public List<SubjectTeacher> SubjectsTeachers { get; set; } = new List<SubjectTeacher>();
        [NotMapped]
        public List<SyllabusManagerUser> Teachers { get; set; } = new List<SyllabusManagerUser>();
        public Specialization Specialization { get; set; }
        [NotMapped]
        public bool IsAdmin { get; set; }
        [NotMapped]
        public bool IsSupervisor { get; set; }
        [NotMapped]
        public bool IsTeacher { get; set; }
    }
}
