using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Logic.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;
using User = SyllabusManager.Data.Models.User.SyllabusManagerUser;
using System.Collections.Generic;

namespace SyllabusManager.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly SyllabusManagerDbContext _dbContext;
        private readonly DbSet<User> _dbSet;

        public UserService(SyllabusManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<User>();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _dbSet.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.Where(u => u.NormalizedEmail == email.ToUpper()).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateAsync(User user)
        {
            // ToDo: remove roles from dbUser
            User dbUser = await _dbSet.FindAsync(user.Id);
            if (dbUser == null)
            {
                return null;
            }
            _dbContext.Entry(dbUser).CurrentValues.SetValues(user);
            await _dbContext.SaveChangesAsync();
            return dbUser;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbSet.Where(u => !u.IsDeleted).AsNoTracking().ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            user.IsDeleted = false;
            User dbUser = await GetByEmailAsync(user.Email);
            if (dbUser != null)
            {
                return null;
            }
            await _dbSet.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            User dbUser = await _dbSet.FindAsync(id);
            if (dbUser == null)
            {
                return false;
            }
            dbUser.Email = null;
            dbUser.PasswordHash = null;
            dbUser.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
