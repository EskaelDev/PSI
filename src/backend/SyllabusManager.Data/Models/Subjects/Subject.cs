using SyllabusManager.Data.Enums.FieldOfStudies;
using SyllabusManager.Data.Enums.Subjects;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Data.Models.ManyToMany;
using SyllabusManager.Data.Models.User;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SyllabusManager.Data.Models.Subjects
{
    public class Subject : DocumentInAcademicYearBase
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string NamePl { get; set; }
        public string NameEng { get; set; }
        public ModuleType ModuleType { get; set; }
        public KindOfSubject KindOfSubject { get; set; }
        public MainLanguage Language { get; set; }
        public TypeOfSubject TypeOfSubject { get; set; }
        public SyllabusManagerUser Supervisor { get; set; }
        public List<Literature> Literature { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<LearningOutcomeEvaluation> LearningOutcomeEvaluations { get; set; }
        public List<CardEntries> CardEntries { get; set; }
        public List<SubjectTeacher> SubjectsTeachers { get; set; }
        public IEnumerable<SyllabusManagerUser> Teachers => SubjectsTeachers.Select(st => st.Teacher);
        public FieldOfStudy FieldOfStudy{ get; set; }
        public Specialization Specialization{ get; set; }
    }
}
