using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Model
{
	class Question
	{
		public long QuestionId { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public QuestionProvider Provider { get; set; }
		public IList<string> Tags { get; set; }

		public IDictionary<string, string> TagUrls { get; set; }
	}
}
