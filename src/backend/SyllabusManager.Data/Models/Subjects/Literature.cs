using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Subjects
{
    public class Literature : ModelBase
    {
        [Required]
        public string Authors { get; set; }
        [Required]
        public string Title { get; set; }
        public string Distributor { get; set; }
        public int? Year { get; set; }
        public bool IsPrimary { get; set; }
        [Required]
        public string Isbn { get; set; }
    }
}
