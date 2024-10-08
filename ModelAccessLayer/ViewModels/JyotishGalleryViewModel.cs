using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishGalleryViewModel
    {
        [AllowNull]
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public IFormFile ImageUrl { get; set; }
        public int JyotishId { get; set; }
    }
}
