using System.Collections.Generic;

namespace SyllabusManager.Logic.Models
{
    public class RegistrationModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

    }
}
