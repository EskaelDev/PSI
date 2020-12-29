using SyllabusManager.Data.Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Interfaces
{
    public interface IUserService
    {
        Task<SyllabusManagerUser> AddAsync(SyllabusManagerUser user);
        Task<List<SyllabusManagerUser>> GetAllAsync();
        Task<SyllabusManagerUser> GetByEmailAsync(string email);
        Task<SyllabusManagerUser> GetByIdAsync(string id);
        Task<SyllabusManagerUser> UpdateAsync(SyllabusManagerUser user);
        Task<bool> DeleteAsync(string id);
    }
}
