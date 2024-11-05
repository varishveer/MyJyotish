using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class DocumentViewModel
    {
       
        
        public IFormFile? IdProof { get; set; }
        public IFormFile? AddressProof { get; set; }
        public IFormFile? TenthCertificate { get; set; }
        public IFormFile? TwelveCertificate { get; set; }
        public IFormFile? ProfessionalCertificate { get; set; }
        public int JyotishId { get; set; }
    }
}
