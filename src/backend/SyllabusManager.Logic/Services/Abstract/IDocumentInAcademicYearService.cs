using SyllabusManager.Data.Models;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services.Abstract
{
    public interface IDocumentInAcademicYearService<T> : IModelBaseService<T> where T : DocumentInAcademicYearBase
    {
        Task<bool> Delete(string id);
    }
}