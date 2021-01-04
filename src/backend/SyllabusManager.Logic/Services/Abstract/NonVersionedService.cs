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
    public abstract class NonVersionedService<T> : INonVersionedService<T> where T : NonVersionedModelBase
    {
        protected readonly SyllabusManagerDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public NonVersionedService(SyllabusManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.Where(e => !e.IsDeleted).AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstAsync(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }


        public virtual async Task<List<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<T> FindAsync(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Adds or Updates if entity already in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<T> SaveAsync(T entity)
        {
            T entityDb = _dbSet.Find(entity.Id);
            if (entityDb == null)
            {
                await _dbSet.AddAsync(entity);
            }
            else
                _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> GetByCodeAsync(string code)
        {
            return await _dbSet.Where(e => e.Id.ToString() == code).AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<T> UpdateAsync(T entity)
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


        public virtual async Task<bool> DeleteAsync(string id)
        {
            T dbEntity = await _dbSet.FindAsync(id);
            if (dbEntity == null)
            {
                return false;
            }
            _dbSet.Remove(dbEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public virtual async Task<bool> SoftDeleteAsync(string id)
        {
            T dbEntity = await _dbSet.FindAsync(id);
            if (dbEntity == null)
            {
                return false;
            }
            dbEntity.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public virtual async Task<bool> SoftDeleteAsync(T entity)
        {
            T dbEntity = await _dbSet.FindAsync(entity.Id);
            if (dbEntity == null)
            {
                return false;
            }
            dbEntity.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
