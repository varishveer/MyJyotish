using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishUserAttachmentJyotishUpdateViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
    }
}
