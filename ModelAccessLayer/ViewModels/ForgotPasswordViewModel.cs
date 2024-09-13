using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public string? Email { get; set; }
        public int Otp { get; set; }
        public string? Password { get; set; }
    }
}
