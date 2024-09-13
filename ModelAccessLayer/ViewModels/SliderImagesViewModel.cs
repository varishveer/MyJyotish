using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class SliderImagesViewModel
    {
        [AllowNull]
        public IFormFile? HomePage { get; set; }
        [AllowNull]
        public IFormFile? BookPoojaCategory { get; set; }
        [AllowNull]
        public IFormFile? PoojaList { get; set; }
    }
}
