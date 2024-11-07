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
        public string? IdProofStatus { get; set; }
        [AllowNull]

        public string? IdProofMessage { get; set; }
        [AllowNull]
        public string? AddressProof { get; set; }
        public string? AddressProofStatus { get; set; }
        [AllowNull]
        public string? AddressProofMessage { get; set; }

        [AllowNull]
        public string? TenthCertificate { get; set; }
        public string? TenthCertificateStatus { get; set; }
        [AllowNull]

        public string? TenthCertificateMessage { get; set; }
        [AllowNull]
        public string? TwelveCertificate { get; set; }
        public string? TwelveCertificateStatus { get; set; }
        [AllowNull]

        public string? TwelveCertificateMessage { get; set; }
        [AllowNull]
        public string? ProfessionalCertificate { get; set; }
        public string? ProfessionalCertificateStatus { get; set; }
        [AllowNull]

        public string? ProfessionalCertificateMessage { get; set; }
        public int JId { get; set; }
        public JyotishModel Jyotish { get; set; }
    }
}
