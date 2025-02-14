using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishVideosViewModel
    {
        [Key]
        public int Id { get; set; }
        public string VideoTitle { get; set; }
        public IFormFile? Image { get; set; }
        public string VideoUrl { get; set; }
        public string SerialNo { get; set; }
        public int JyotishId { get; set; }
    }
}
