using SyllabusManager.Data.Models.Syllabuses;

namespace SyllabusManager.Logic.Pdf
{
    public interface IPlanPdf
    {
        void Create(Syllabus syllabus);
    }
}