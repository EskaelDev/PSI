using SyllabusManager.Logic.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> AddAsync(UserDTO user);
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByEmailAsync(string email);
        Task<UserDTO> GetByIdAsync(string id);
        Task<UserDTO> AddOrUpdateAsync(UserDTO user);
        Task<bool> DeleteAsync(string id);
    }
}
