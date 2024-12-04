using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class DepartmentPagesValidation
    {
        public int Id { get; set; } 
        public int DepartmentId { get; set; }
        public int PageId { get; set; }
        public bool status { get; set; }

        public Department department { get; set; }
        public EmployeesAccessPages pages { get; set; }
    }
}
