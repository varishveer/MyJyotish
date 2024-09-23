using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class EmailDocumentViewModel
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public string? Image { get; set; }


    }
}
