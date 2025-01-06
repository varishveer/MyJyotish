using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class UserServiceRecordModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TimeOfBirth { get; set; }
        public string PlaceOfBirth { get; set;  }
        public int UserId { get; set;  }
        public int JyotishId { get; set; }
        public int Action { get; set; }
        public int Count { get; set; }
        public DateTime date { get; set; }
        public bool Status { get; set; }

        public UserModel User { get; set; }
        public JyotishModel Jyotish { get; set; }



    }
}
