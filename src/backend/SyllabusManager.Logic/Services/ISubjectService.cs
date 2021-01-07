using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Logic.Services
{
    public interface ISubjectService : IDocumentInAcademicYearService<Subject>
    {
        Task<List<Subject>> GetAll(string fos, string spec, string year);
        Task<List<Subject>> GetAllForUser(string fos, string spec, string year, SyllabusManagerUser user, bool onlyMy);
        Task<List<string>> History(Guid id);
        Task<Subject> ImportFrom(Guid currentDocId, string fosCode, string specCode, string code, string academicYear);
        Task<Subject> Latest(string fos, string spec, string code, string year);
        Task<Subject> Save(Subject syllabus);
        Task<bool> Delete(Guid id);
    }
}
