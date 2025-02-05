using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class PoojaBookMark
    {
        public int Id { get; set; }
        public int poojaId {get;set;}
        public string BookMark { get; set; }
        public DateTime EndDate { get; set; }

        public bool status { get; set; }
        public BookedPoojaList Pooja { get; set; }
    }
}
