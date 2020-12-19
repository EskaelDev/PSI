using SyllabusManager.Data.Enums.Subjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Subjects
{
    public class CardEntries : ModelBase
    {
        [Required]
        public string Name { get; set; }
        public SubjectCardEntryType Type { get; set; }
        public List<CardEntry> Entries { get; set; }
    }
}
