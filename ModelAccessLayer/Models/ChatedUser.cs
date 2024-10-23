using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ModelAccessLayer.Models
{
    public class ChatedUser
    {
        [Key]
        public int Id { get; set; }
       
        public DateTime FirstMessageAt { get; set; }
        public DateTime LastMessageAt { get; set; }
        public string UserType { get; set; }
        public int Status { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
       
        public int JyotishId { get; set; }
        public JyotishModel Jyotish { get; set; }
    }
}
