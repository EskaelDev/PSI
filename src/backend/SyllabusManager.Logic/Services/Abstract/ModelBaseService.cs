using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services.Abstract
{
    public abstract class ModelBaseService<T> : IModelBaseService<T> where T : ModelBase
    {
        protected readonly SyllabusManagerDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public ModelBaseService(SyllabusManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstAsync(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }


        public virtual async Task<List<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> FindAsync(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.Where(e => e.Id.ToString() == id).FirstOrDefaultAsync();
        }

        public virtual async Task<T> Update(T entity)
        {
            T dbEntity = await _dbSet.FindAsync(entity.Id);
            if (dbEntity == null)
            {
                return null;
            }
            _dbContext.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return dbEntity;
        }
    }
}
