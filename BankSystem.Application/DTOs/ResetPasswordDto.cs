﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Application.DTOs
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }         
        public string NewPassword { get; set; }
    }
}
