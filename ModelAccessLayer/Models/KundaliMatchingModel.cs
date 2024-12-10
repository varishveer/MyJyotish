using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class KundaliMatchingModel
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public TimeOnly TimeOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateTime { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }
        public UserModel User { get; set; }
    }
}
