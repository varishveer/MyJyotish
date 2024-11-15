using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public  class SubscriptionFeatureViewModel
    {
        public class Add
        {
            public string Name { get; set; }
            public string ServiceUrl { get; set; }
        }

        public class Update
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ServiceUrl { get; set; }
        }

        public class GetAll
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ServiceUrl { get; set; }
        }

    }
}
