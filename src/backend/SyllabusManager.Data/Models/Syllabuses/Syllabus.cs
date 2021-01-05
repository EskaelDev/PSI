using SyllabusManager.Data.Enums.Syllabuses;
using SyllabusManager.Data.Models.FieldOfStudies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Syllabuses
{

    /// <summary>
    /// Program studiów
    /// </summary>
    public class Syllabus : DocumentInAcademicYearBase
    {
        public State State { get; set; } = State.Draft;
        public Option? StudentGovernmentOpinion { get; set; }
        public DateTime? OpinionDeadline { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? ValidFrom { get; set; }
        public string StudentRepresentativeName { get; set; }
        public string DeanName { get; set; }
        [Required] 
        public string AuthorName { get; set; } = "unknown";
        [Required]
        public string ScopeOfDiplomaExam { get; set; }
        public string IntershipType { get; set; }
        public SyllabusDescription Description { get; set; } = new SyllabusDescription();
        public List<SubjectInSyllabusDescription> SubjectDescriptions { get; set; } = new List<SubjectInSyllabusDescription>();
        public FieldOfStudy FieldOfStudy { get; set; }
        public Specialization Specialization { get; set; }
    }
}
