using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class ActiveMail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public bool ActiveStatus { get; set; }
    }
}
