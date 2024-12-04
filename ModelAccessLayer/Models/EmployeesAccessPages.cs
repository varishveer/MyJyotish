using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class EmployeesAccessPages
    {
        public int Id { get; set; }
        public string pagesName { get; set; }
        public string pageUrl { get; set; }
        public bool status { get; set; }
    }
}
