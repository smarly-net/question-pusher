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
	class TosterParseService : IStartService
	{
		private IQuestionRepository _repository;
		private ITosterParse _tosterParse;
		private IErrorProvider _errorProvider;
		private IPushAllProvider _pushAllProvider;

		public TosterParseService(ITosterParse tosterParse, IQuestionRepository repository, IErrorProvider errorProvider, IPushAllProvider pushAllProvider)
		{
			if (tosterParse == null)
			{
				throw new ArgumentNullException("tosterParse");
			}
			_tosterParse = tosterParse;

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
					questions.AddRange(await _tosterParse.ParseList());
					var resultQuestions = await _repository.AddQuestions(questions);

					await _tosterParse.ParseTags(resultQuestions);
					var tags = await _repository.UpdateTagsAndStatus(resultQuestions);

					if (tags.Any())
					{
						Dictionary<string, string> tagUrls = new Dictionary<string, string>();

						foreach (var pair in resultQuestions.Where(x => x.TagUrls != null).SelectMany(x => x.TagUrls))
						{
							if (!tagUrls.ContainsKey(pair.Key))
							{
								tagUrls[pair.Key] = pair.Value;
							}
						}

						foreach (var tag in tags)
						{
							tag.Url = tagUrls[tag.Name];
						}

						await _tosterParse.ParseTagImage(tags);

						await _repository.UpdateTagImage(tags);
					}

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
