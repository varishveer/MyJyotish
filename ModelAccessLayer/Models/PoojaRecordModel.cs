using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class PoojaRecordModel
    {
        [Key]
        public int Id { get; set; }
      
        public int PoojaType { get; set; }
        public int jyotishId { get; set; }
        public string title { get; set; }
        public string Procedure { get; set; }
        public string Benefits { get; set; }
        public string AboutGod { get; set; }
        [AllowNull]
        public string Image { get; set; }
        public bool status { get; set; }

        public PoojaListModel pooja { get; set; }
        public JyotishModel jyotish { get; set; }
        public ICollection<BookedPoojaList> BookedPooja { get; set; } = new List<BookedPoojaList>();


    }
}
