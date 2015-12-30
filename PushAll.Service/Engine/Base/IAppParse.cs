using PushAll.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Engine.Base
{
	internal interface IAppParse
	{
		Task<IList<Question>> ParseList();
		Task ParseTags(IList<Question> questions);
		Task ParseTagImage(IList<Tag> tags);

	}
}
