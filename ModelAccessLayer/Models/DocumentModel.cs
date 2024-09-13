using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class DocumentModel
    {
        [Key]
        public int Id { get; set; }
        [AllowNull]
        public string? IdProof { get; set; }
        [AllowNull]
        public string? AddressProof { get; set; }
        [AllowNull]
        public string? TenthCertificate { get; set; }
        [AllowNull]
        public string? TwelveCertificate { get; set; }
        [AllowNull]
        public string? ProfessionalCertificate { get; set; }
      /*  public int JId { get; set; }
        public JyotishModel Jyotish { get; set; }*/
    }
}
