using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Extensions;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Models;
using SyllabusManager.Logic.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<SyllabusManagerUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SyllabusManagerDbContext _dbContext;
        private readonly DbSet<SyllabusManagerUser> _dbSet;

        public UserService(UserManager<SyllabusManagerUser> userManager, RoleManager<IdentityRole> roleManager, SyllabusManagerDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<SyllabusManagerUser>();
        }

        public async Task<UserDTO> GetByIdAsync(string id)
        {
            SyllabusManagerUser syllabusUser = await _dbSet.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (syllabusUser is null) return null;
            List<string> roles = (await _userManager.GetRolesAsync(syllabusUser)).ToList();
            return syllabusUser.MakeDto(roles);
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            SyllabusManagerUser syllabusUser = await _dbSet.Where(u => u.Email.ToUpper() == email.ToUpper()).FirstOrDefaultAsync();
            if (syllabusUser is null) return null;
            List<string> roles = (await _userManager.GetRolesAsync(syllabusUser)).ToList();
            return syllabusUser.MakeDto(roles);
        }

        public async Task<UserDTO> AddOrUpdateAsync(UserDTO user)
        {
            SyllabusManagerUser dbUser = await _dbSet.FindAsync(user.Id);
            if (dbUser == null)
            {
                return await AddAsync(user);
            }
            List<string> roles = (await _userManager.GetRolesAsync(dbUser)).ToList();
            await _userManager.RemoveFromRolesAsync(dbUser, roles);
            await _userManager.AddToRolesAsync(dbUser, user.Roles);

            _dbContext.Entry(dbUser).CurrentValues.SetValues(user);
            dbUser.NormalizedEmail = user.Email.ToUpper();

            await _dbContext.SaveChangesAsync();

            roles = (await _userManager.GetRolesAsync(dbUser)).ToList();
            return dbUser.MakeDto(roles);
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            List<SyllabusManagerUser> dbUsers = await _dbSet.Where(u => !u.IsDeleted).AsNoTracking().ToListAsync();
            List<UserDTO> users = new List<UserDTO>();
            foreach (SyllabusManagerUser dbuser in dbUsers)
            {
                users.Add(dbuser.MakeDto((await _userManager.GetRolesAsync(dbuser)).ToList()));
            }
            return users;
        }

        public async Task<UserDTO> AddAsync(UserDTO user)
        {
            UserDTO dbUser = await GetByEmailAsync(user.Email);
            if (dbUser != null)
            {
                return null;
            }
            SyllabusManagerUser syllabusUser = user.MakeSyllabusManagerUser();
            await _userManager.CreateAsync(syllabusUser, "S4#SAX@2WqS?mkr&");
            await _userManager.AddToRolesAsync(syllabusUser, user.Roles);
            return (await _userManager.FindByEmailAsync(user.Email)).MakeDto(user.Roles);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            SyllabusManagerUser dbUser = await _dbSet.FindAsync(id);
            if (dbUser == null)
            {
                return false;
            }
            List<string> roles = (await _userManager.GetRolesAsync(dbUser)).ToList();
            await _userManager.RemoveFromRolesAsync(dbUser, roles);
            dbUser.Email = null;
            dbUser.NormalizedEmail = null;
            dbUser.PasswordHash = null;
            dbUser.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserDTO>> GetByRoleAsync(string role)
        {
            var roleId = (await _roleManager.FindByNameAsync(role))?.Id;
            return _dbContext.Users.Where(u => _dbContext.UserRoles.Any(r => r.RoleId == roleId && r.UserId == u.Id))
                .ToList().Select(u => u.MakeDto(new List<string>())).ToList();
        }

        public async Task<List<UserDTO>> GetTeachers()
        {
            return await GetByRoleAsync(UsersRoles.Teacher);
        }
    }
}
