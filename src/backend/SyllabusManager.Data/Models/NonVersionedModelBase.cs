using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyllabusManager.Data.Models
{
    public class NonVersionedModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Code { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public string Id => Code;
    }
}
