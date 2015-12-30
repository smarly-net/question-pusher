using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Model
{
	class SendingQuestion
	{
		public long QuestionId { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public string ProviderDescription { get; set; }
		public string PushAllUserId { get; set; }
	}
}
