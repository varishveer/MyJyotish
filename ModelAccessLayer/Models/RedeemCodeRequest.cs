using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
	public class RedeemCodeRequest
	{
		public int Id { get; set; }
		public int jyotishId { get; set; }
		public int? EId { get; set; }
		public int planId { get; set; }
		public DateTime RequestDate { get; set; }
		public bool RedeemStatus { get; set; }
		public bool appstatus { get; set; }
		public bool status { get; set; }


		public JyotishModel jyotish { get; set; }
		public SubscriptionModel plan { get; set; }
		public Employees Employee { get; set; }

	}
}
