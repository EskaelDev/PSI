using SyllabusManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services.Abstract
{
    public interface INonVersionedService<T> where T : NonVersionedModelBase
    {
        Task<T> AddAsync(T entity);
        Task<bool> DeleteAsync(string id);
        Task<T> FindAsync(params object[] keys);
        Task<List<T>> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<T> GetByCodeAsync(string id);
        Task<T> SaveAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> SoftDeleteAsync(string id);
        Task<bool> SoftDeleteAsync(T entity);
    }
}