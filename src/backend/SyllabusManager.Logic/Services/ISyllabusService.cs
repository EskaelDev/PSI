using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public interface ISyllabusService : IDocumentInAcademicYearService<Syllabus>
    {
        Task<List<string>> History(Guid id);
        Task<Syllabus> ImportFrom(Guid currentDocId, string fosCode, string specCode, string academicYear);
        Task<Syllabus> Latest(string fos, string spec, string year);
        Task<Syllabus> Save(Syllabus syllabus);
        Task<Syllabus> SaveAs(string fosCode, string specCode, string academicYear, Syllabus syllabus);
    }
}