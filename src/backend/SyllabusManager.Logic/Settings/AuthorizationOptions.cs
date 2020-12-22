using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Settings
{
    public class AuthorizationOptions
    {
        public string Secret { get; set; }
        public int Expiration { get; set; }
    }
}
