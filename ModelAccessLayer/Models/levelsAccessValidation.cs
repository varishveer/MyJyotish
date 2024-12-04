using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class levelsAccessValidation
    {
        public int Id { get; set; }
        public int levels { get; set; }
        public int department { get; set; }
        public int pages { get; set; }
        public bool status { get; set; }
        public Department departmentRelation { get; set; }
        public levels LevelsRelation { get; set; }
        public EmployeesAccessPages EmpPages { get; set; }
    }
}
