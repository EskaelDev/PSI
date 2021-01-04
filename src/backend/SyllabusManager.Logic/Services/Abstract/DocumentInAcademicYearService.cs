using SyllabusManager.Data;
using SyllabusManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services.Abstract
{
    public abstract class DocumentInAcademicYearService<T> : ModelBaseService<T>, IDocumentInAcademicYearService<T> where T : DocumentInAcademicYearBase
    {
        public DocumentInAcademicYearService(SyllabusManagerDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            entity.IsDeleted = true;
            var state = await _dbContext.SaveChangesAsync();
            return state > 0;
        }
        protected string IncreaseVersion(string version)
        {
            string newVersion = DateTime.UtcNow.ToString("yyyyMMdd");

            if (version.Substring(0, 8) == newVersion)
            {
                string currentV = version.Substring(8);
                int newV = int.Parse(currentV) + 1;
                return version.Substring(0, 8) + newV.ToString("00");
            }

            return newVersion + "01";
        }

    }
}
