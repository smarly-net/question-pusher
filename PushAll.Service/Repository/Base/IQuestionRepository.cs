using PushAll.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Repository.Base
{
	interface IQuestionRepository
	{
		Task<IList<Question>> AddQuestions(IList<Question> questions);
		Task<IList<Tag>> UpdateTagsAndStatus(IList<Question> questions);
		Task UpdateTagImage(IList<Tag> tags);
		Task<IList<SendingQuestion>> GetQuestionForSend();
	}
}
