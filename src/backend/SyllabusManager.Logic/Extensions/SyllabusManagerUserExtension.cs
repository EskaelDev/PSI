using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Models.DTO;
using System.Collections.Generic;

namespace SyllabusManager.Logic.Extensions
{
    public static class SyllabusManagerUserExtension
    {
        public static UserDTO MakeDto(this SyllabusManagerUser syllabusUser, List<string> roles)
        {
            return new UserDTO
            {
                Email = syllabusUser.Email,
                Id = syllabusUser.Id,
                Roles = roles,
                Name = syllabusUser.Name

            };
        }
    }
}
