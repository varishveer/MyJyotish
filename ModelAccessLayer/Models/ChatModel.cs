using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public int senderId { get; set; }
        public int receiverId { get; set; }
        public string message { get; set; }
        public string mssDate { get; set; }
        public string sendBy { get; set; }
        public int status { get; set; }

    }
}
