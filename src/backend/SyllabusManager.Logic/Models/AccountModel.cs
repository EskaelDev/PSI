using System;
using System.Collections.Generic;
using System.Text;

namespace SyllabusManager.Logic.Models
{
    public class AccountModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IList<string> Roles { get; set; }
    }
}
