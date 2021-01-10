using System.ComponentModel.DataAnnotations;
using SyllabusManager.Data.Models.FieldOfStudies;

namespace SyllabusManager.Data.Models
{
    public class DocumentInAcademicYearBase : ModelBase
    {
        [Required]
        public string AcademicYear { get; set; }
        /// <summary>
        /// YYYYMMdd{nr}
        /// </summary>
        [Required]
        public string Version { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
