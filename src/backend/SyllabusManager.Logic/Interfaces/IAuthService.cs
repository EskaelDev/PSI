using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Models;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Interfaces
{
    public interface IAuthService
    {
        public Task<IdentityResult> RegisterUser(RegistrationModel registrationModel);
        public Task<UserCredentialsModel> Login(SyllabusManagerUser user);
    }
}