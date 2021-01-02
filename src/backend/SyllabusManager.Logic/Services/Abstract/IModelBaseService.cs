using SyllabusManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services.Abstract
{
    public interface IModelBaseService<T> where T : ModelBase
    {
        Task<T> AddAsync(T entity);
        Task<T> FindAsync(params object[] keys);
        Task<List<T>> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> Update(T entity);
    }
}