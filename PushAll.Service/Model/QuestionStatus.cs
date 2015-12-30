using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Model
{
	enum QuestionStatus : short
	{
		InProgress = 1,
		ReadyToSend = 2,
		Sending = 3,
		Complete = 4,
	}
}
