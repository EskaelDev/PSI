using SyllabusManager.Data.Attributes;
using SyllabusManager.Data.Enums.LearningOutcomes;
using SyllabusManager.Data.Models.FieldOfStudies;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.LearningOutcomes
{
    public class LearningOutcome : ModelBase
    {
        [Required]
        [PdfName("Symbol kierunkowych efektów uczenia się")]
        public string Symbol { get; set; }
        public LearningOutcomeCategory Category { get; set; }
        [PdfName("Opis efektów uczenia się dla kierunku studiów")]
        public string Description { get; set; }
        [Required]
        [PdfName("Uniwersalne charakterystyki pierwszego stopnia(U)")]
        public string U1degreeCharacteristics { get; set; }
        [PdfName("Charakterystyki dla kwalifikacji na poziomie 6 PRK")]
        public string S2degreePrk { get; set; }
        [PdfName("Charakterystyki dla kwalifikacji na poziomie 6 PRK, umożliwiających uzyskanie kompetencji inżynierskich")]
        public string S2degreePrkeng { get; set; }

        public Specialization Specialization { get; set; }
    }
}
