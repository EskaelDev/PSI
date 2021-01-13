using SyllabusManager.Data.Models.Subjects;

namespace SyllabusManager.Logic.Pdf
{
    public interface ISubjectPdf
    {
        void Create(Subject subject);
    }
}