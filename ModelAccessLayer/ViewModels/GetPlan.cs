using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class getPlan
    {
        public List<ListData> Subscription { get; set; }

    }

    public class ListData {

        public string Name { get; set; }
        public string Url { get; set; }

        public int SubscriptionId { get; set; }
    }

}
