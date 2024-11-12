using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class ClientMembersViewModel
    {
        public int Id { get; set; }
        public int appointmentId { get; set; }
        public string? Name { get; set; }
        public string? dob { get; set; }
        public string? gender { get; set; }
        public string? relation { get; set; }
        public int status { get; set; }
    }
}
