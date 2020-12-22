﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SyllabusManager.Logic.Models
{
    public class UserCredentialsModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public AccountModel Account { get; set; }
    }
}
