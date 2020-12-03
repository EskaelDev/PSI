using SyllabusManager.Data.Enums.Syllabuses;
using SyllabusManager.Data.Models.FieldOfStudies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Syllabuses
{
    public partial class Syllabus : DocumentInAcademicYearBase
    {
        public Option StudentGovernmentOpinion { get; set; }
        public DateTime? OpinionDeadline { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public string StudentRepresentativeName { get; set; }
        [Required]
        public string DeanName { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public string ScopeOfDiplomaExam { get; set; }
        public string IntershipType { get; set; }
        public SyllabusDescription Description { get; set; }
        public List<SubjectInSyllabusDescription> SubjectDescriptions { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
    }
}
