using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class jyotishSessions
    {

       public int Id { get; set; }
       public int session_id { get; set; } 
       public int jyotishId { get; set; }
       public bool status { get; set; }
        public JyotishModel jyotish { get; set; }

    }
}
