using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
	public class JyotishPooja
	{
		public int Id { get; set; }
		public int JyotishId { get; set; }
		public int poojaType { get; set; }
		public int amount { get; set; }
		public DateTime date { get; set; }
		public bool status { get; set; }
		public JyotishModel jyotish { get; set; }
		public PoojaListModel pooja { get; set; }
	}
}
