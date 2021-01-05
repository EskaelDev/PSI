using SyllabusManager.Data.Models;
using System;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services.Abstract
{
    public interface IDocumentInAcademicYearService<T> : IModelBaseService<T> where T : DocumentInAcademicYearBase
    {
        Task<bool> Delete(Guid id);
    }
}