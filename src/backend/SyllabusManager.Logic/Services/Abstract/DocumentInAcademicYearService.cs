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

        public async Task<bool> Delete(string id)
        {
            var entity = _dbSet.Find(id);
            entity.IsDeleted = true;
            var state = await _dbContext.SaveChangesAsync();
            return state > 0;
        }


    }
}
