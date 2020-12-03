using SyllabusManager.Data.Enums.LearningOutcomes;
using SyllabusManager.Data.Models.FieldOfStudies;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.LearningOutcomes
{
    public partial class LearningOutcome : ModelBase
    {
        [Required]
        public string Symbol { get; set; }
        public LearningOutcomeCategory Category { get; set; }
        public string Description { get; set; }
        [Required]
        public string U1degreeCharacteristics { get; set; }
        public string S2degreePrk { get; set; }
        public string S2degreePrkeng { get; set; }
        public Specialization Specialization { get; set; }
    }
}
