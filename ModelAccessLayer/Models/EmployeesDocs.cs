using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class EmployeesDocs
    {
        public int Id { get; set; }
        public int employees { get; set; }
        public string DocsName { get; set; }
        public string DocUrl { get; set; }
        public bool status { get; set; }

        public Employees employeeRecord { get; set; }
    }
}
