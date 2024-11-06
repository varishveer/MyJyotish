using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class UploadProfileImagePJViewModel
    {
        public int Id { get; }
        public IFormFile Image { get; set; }
    }
}
