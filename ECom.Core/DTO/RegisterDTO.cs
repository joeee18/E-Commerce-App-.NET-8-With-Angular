﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.DTO
{
    public record LoginDTO
    {  
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public record RegisterDTO:LoginDTO
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }


    }

    public record resetPasswordDTO : LoginDTO
    {
        public string Token { get; set; }
    }
    public record ActiveAccountDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
    
}
