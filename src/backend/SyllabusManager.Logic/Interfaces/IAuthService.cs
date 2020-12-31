using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Models;
using SyllabusManager.Logic.Models.DTO;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Interfaces
{
    public interface IAuthService
    {
        public Task<UserDTO> RegisterUser(RegistrationModel registrationModel);
        public Task<UserCredentialsModel> Login(SyllabusManagerUser user);
    }
}