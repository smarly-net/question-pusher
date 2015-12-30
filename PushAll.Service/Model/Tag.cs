using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Model
{
	class Tag
	{
		public string Name { get; set; }
		public int TagId { get; set; }
		public byte[] Image { get; set; }
		public string Url { get; set; }
		public int Subscribers { get; internal set; }
	}
}
