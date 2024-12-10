using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class PoojaCategoryViewModel
    {
        [AllowNull]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
