using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class JyotishUserAttachmentViewModel
    {
        [AllowNull]
        public int? Id { get; set; }
        public int JyotishId { get; set;  }
        public int  UserId { get; set; }
        public IFormFile Image { get; set; }
    }
}
