using SyllabusManager.Data.Attributes;
using SyllabusManager.Data.Enums.LearningOutcomes;
using SyllabusManager.Data.Models.FieldOfStudies;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.LearningOutcomes
{
    public class LearningOutcome : ModelBase
    {
        [Required]
        [PdfName("Symbol")]
        public string Symbol { get; set; }
        [PdfName("Kategoria")]
        public LearningOutcomeCategory Category { get; set; }
        [PdfName("Opis")]
        public string Description { get; set; }
        [Required]
        [PdfName("U1 stopien")]
        public string U1degreeCharacteristics { get; set; }
        [PdfName("S2 Prk")]
        public string S2degreePrk { get; set; }
        [PdfName("S2 Prk eng")]
        public string S2degreePrkeng { get; set; }

        public Specialization Specialization { get; set; }
    }
}
