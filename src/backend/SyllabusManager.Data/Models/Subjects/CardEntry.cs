using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Subjects
{
    public class CardEntry : ModelBase
    {
        [Required]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
