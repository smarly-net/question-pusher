using PushAll.Service.Engine.Base;
using PushAll.Service.Model;
using PushAll.Service.Provider;
using PushAll.Service.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PushAll.Service.Services
{
	class StackoverflowParseService : IStartService
	{
		private IQuestionRepository _repository;
		private IStackoverflowParse _stackoverflowParse;
		private IErrorProvider _errorProvider;
		private IPushAllProvider _pushAllProvider;

		public StackoverflowParseService(IStackoverflowParse stackoverflowParse, IQuestionRepository repository, IErrorProvider errorProvider, IPushAllProvider pushAllProvider)
		{
			if (stackoverflowParse == null)
			{
				throw new ArgumentNullException("stackoverflowParse");
			}
			_stackoverflowParse = stackoverflowParse;

			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			_repository = repository;

			if (errorProvider == null)
			{
				throw new ArgumentNullException("errorProvider");
			}
			_errorProvider = errorProvider;

			if (pushAllProvider == null)
			{
				throw new ArgumentNullException("pushAllProvider");
			}
			_pushAllProvider = pushAllProvider;
		}

		public async void Start(string[] args)
		{
			List<Question> questions = new List<Question>();
			while (true)
			{
				try
				{
					questions.AddRange(await _stackoverflowParse.ParseList());
					var resultQuestions = await _repository.AddQuestions(questions);

					foreach (var q in resultQuestions)
					{
						q.Tags = questions.Single(x => x.Url == q.Url).Tags;
					}

					var tags = await _repository.UpdateTagsAndStatus(resultQuestions);

					if (resultQuestions.Any())
					{
						await _pushAllProvider.Send();
					}
				}
				catch (Exception ex)
				{
					_errorProvider.SendError(ex);
				}
				finally
				{
					questions.Clear();
					Thread.Sleep(60 * 1000);
				}
			}
		}
	}
}
