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
        public int JyotishId { get; set;  }
        public int  UserId { get; set; }
        public int member { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
