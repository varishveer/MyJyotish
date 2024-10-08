using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class JyotishGalleryModel
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImageUrl { get; set; }
        public int JyotishId { get; set; }
        public JyotishModel Jyotish { get; set; }

    }
}
