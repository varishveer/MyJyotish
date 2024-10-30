using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class JyotishUserAttachmentJyotishViewModel
    {
        public int Id { get; set; }
        public int JyotishId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
