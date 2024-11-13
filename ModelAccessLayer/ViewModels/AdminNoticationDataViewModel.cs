using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class AdminNoticationDataViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date {  get; set; }   
        public string Time { get; set; }
        public int TimeDuration { get; set; }
    }
}
