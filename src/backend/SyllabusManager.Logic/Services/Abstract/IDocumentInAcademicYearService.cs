using System;
using System.Threading.Tasks;
using SyllabusManager.Data.Models;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Logic.Services.Abstract
{
    public interface IDocumentInAcademicYearService<T> : IModelBaseService<T> where T : DocumentInAcademicYearBase
    {
        Task<bool> CheckIfFosSupervisor(SyllabusManagerUser user, string fosCode);
        Task<bool> CheckIfFosSupervisor(SyllabusManagerUser user, Guid documentId);
    }
}