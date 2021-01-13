using SyllabusManager.Data.Attributes;
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
        [PdfName("Nazwa")]
        public string Name { get; set; }
        [PdfName("Poziom")]
        public DegreeLevel Level { get; set; }
        [PdfName("Profil")]
        public StudiesProfile Profile { get; set; }
        [PdfName("Dziedzina nauki")]
        public string BranchOfScience { get; set; }
        [PdfName("Dyscyplina")]
        public string Discipline { get; set; }
        [Required]
        [PdfName("Wydział")]
        public string Faculty { get; set; } = "Informatyka i Zarządzanie";
        [PdfName("Typ kursu")]
        public CourseType Type { get; set; }
        [PdfName("Język")]
        public MainLanguage Language { get; set; }
        public SyllabusManagerUser Supervisor { get; set; }
        public List<Specialization> Specializations { get; set; } = new List<Specialization>();
    }
}