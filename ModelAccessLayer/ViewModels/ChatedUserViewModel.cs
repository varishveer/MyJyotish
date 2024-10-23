using ModelAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class ChatedUserViewModel
    {

        public int Id { get; set; }

        public DateTime FirstMessageAt { get; set; }
        public DateTime LastMessageAt { get; set; }

        public string UserType { get; set; }
        public int Status { get; set; }

        public int UserId { get; set; }
   

        public int JyotishId { get; set; }
       
    }
}
