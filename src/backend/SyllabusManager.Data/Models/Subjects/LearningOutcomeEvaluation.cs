using SyllabusManager.Data.Enums.Subjects;
using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Subjects
{
    public class LearningOutcomeEvaluation : ModelBase
    {
        [Required]
        public GradingSystem GradingSystem { get; set; }
        [Required]
        public string LearningOutcomeSymbol { get; set; }
        public string Description { get; set; }
    }
}
