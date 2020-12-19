using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SyllabusManager.Data.Models
{
    public class NonVersionedModelBase
    {
        [Key]
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
    }
}
