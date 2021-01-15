using SyllabusManager.Data.Models.LearningOutcomes;

namespace SyllabusManager.Logic.Pdf
{
    public interface ILearningOutcomePdf
    {
        void Create(LearningOutcomeDocument lod);
    }
}