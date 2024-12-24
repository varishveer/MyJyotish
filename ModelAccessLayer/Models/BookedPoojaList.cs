using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class BookedPoojaList
    {
        public int Id { get; set; }
        public int PoojaId { get; set; }
        public int jyotishId { get; set; }
        public int userId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime PoojaDate { get; set; }
        public bool status { get; set; }

        public UserModel User { get; set; }
        public JyotishModel Jyotish { get; set; }
        public PoojaRecordModel Pooja { get; set; }
    }
}
