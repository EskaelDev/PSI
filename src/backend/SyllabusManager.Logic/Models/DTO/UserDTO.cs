using SyllabusManager.Data.Models.User;
using System.Collections.Generic;

namespace SyllabusManager.Logic.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }


        public SyllabusManagerUser MakeSyllabusManagerUser()
        {
            return new SyllabusManagerUser
            {
                Email = Email,
                UserName = UserName,
                IsDeleted = false,

            };
        }
    }
}
