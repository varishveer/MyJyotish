using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class JyotishVideosModel
    {

        public int Id { get; set; }
        public string VideoTitle { get; set; }
        public string VideoUrl { get; set; }
        public int JyotishId { get; set; }
        public JyotishModel Jyotish { get; set; }
    }
}
