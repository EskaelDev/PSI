using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.FieldOfStudies
{
    public class Specialization : NonVersionedModelBase
    {
        [Required]
        public string Name { get; set; }

        public FieldOfStudy FieldOfStudy { get; set; }

    }
}
