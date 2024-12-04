using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName {get;set;}

        public bool status { get; set; }
        public ICollection<Employees> employees { get; set; } = new List<Employees>();
        public ICollection<DepartmentPagesValidation> EmpPages { get; set; } = new List<DepartmentPagesValidation>();
        public ICollection<levelsAccessValidation> LevelAccess { get; set; } = new List<levelsAccessValidation>();


    }
}
