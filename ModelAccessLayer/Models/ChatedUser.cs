using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class ChatedUser
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public DateTime firstMessageAt { get; set; }
        public DateTime lastMessageAt { get; set; }
        public string userType { get; set; }
        public int status { get; set; }
    }
}
