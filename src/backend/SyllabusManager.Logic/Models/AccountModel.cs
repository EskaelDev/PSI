using System.Collections.Generic;

namespace SyllabusManager.Logic.Models
{
    public class AccountModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IList<string> Roles { get; set; }
    }
}
