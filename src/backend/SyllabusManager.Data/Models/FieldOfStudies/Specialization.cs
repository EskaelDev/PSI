using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.FieldOfStudies
{
    public partial class Specialization
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
