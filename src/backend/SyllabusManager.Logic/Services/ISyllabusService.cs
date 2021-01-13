using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public interface ISyllabusService : IDocumentInAcademicYearService<Syllabus>
    {
        Task<List<string>> History(Guid id);
        Task<Syllabus> ImportFrom(Guid currentDocId, string fosCode, string specCode, string academicYear, SyllabusManagerUser user);
        Task<Syllabus> Latest(string fos, string spec, string year);
        Task<Syllabus> Save(Syllabus syllabus, SyllabusManagerUser user);
        Task<Syllabus> SaveAs(string fosCode, string specCode, string academicYear, Syllabus syllabus, SyllabusManagerUser user);
        Task<bool> Delete(Guid id);
        Task<bool> Pdf(Guid id);
        Task<bool> Pdf(string fos, string spec, string year);


        Task<bool> PlanPdf(Guid id);
    }
}