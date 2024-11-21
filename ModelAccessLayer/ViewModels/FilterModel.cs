using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class FilterModel
    {
        public int id { get; set; }
        [AllowNull]

        public int country { get; set; }
        [AllowNull]

        public int city { get; set; }
        [AllowNull]

        public int state { get; set; }
        [AllowNull]

        public string? gender { get; set; }
        [AllowNull]

        public int rating { get; set; }
        [AllowNull]

        public int activity { get; set; }
        [AllowNull]
        public string? experience { get; set; }
    }
}
