using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class Employees
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Mobile { get; set; }
        public string Email { get; set; }
        public string gender { get; set; }
        public string DateOfBirth { get; set; }
        public int Department { get; set; }
        public int levels { get; set; }
        public DateTime AddingDate { get; set; }
        public bool status { get; set; }

        public Department departmentRelation { get; set; }
        public levels LevelsRelation { get; set; }

        public ICollection<EmployeesDocs> employeeDocs { get; set; } = new List<EmployeesDocs>();

      
    }
}
