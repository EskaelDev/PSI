using System.ComponentModel.DataAnnotations;

namespace SyllabusManager.Data.Models.Subjects
{
    public partial class LearningOutcomeEvaluation : ModelBase
    {
        [Required]
        public string GradingSystem { get; set; }
        [Required]
        public string LearningOutcomeSymbol { get; set; }
        public string Description { get; set; }
    }
}
