using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public abstract class AbstractCrudService<T> where T : class
    {
        private readonly SyllabusManagerDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public AbstractCrudService(SyllabusManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<List<T>> GetAll()
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

        public async Task<T> FindAsync(params object[] keys)
        {
            return await _dbSet.FindAsync(keys);
        }

        public virtual async Task AddAsync(T entity)
        {
            if(entity.Id)
            await _dbSet.AddAsync(entity);
        }
    }
}
