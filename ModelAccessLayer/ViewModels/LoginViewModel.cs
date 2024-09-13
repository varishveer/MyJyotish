using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
