using PushAll.Service.App_Start;
using PushAll.Service.Model;
using PushAll.Service.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PushAll.Service.Provider
{
	class PushAllProvider : IPushAllProvider
	{
		private IQuestionRepository _repository;
		private IErrorProvider _errorProvider;

		public PushAllProvider(IQuestionRepository repository, IErrorProvider errorProvider)
		{
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
		}

		public async Task Send()
		{
			try
			{
				IList<SendingQuestion> questions = await _repository.GetQuestionForSend();

				Dictionary<long, List<SendingQuestion>> qs = questions
					.GroupBy(x => x.QuestionId)
					.ToDictionary(k => k.Key, v => questions.Where(x => x.QuestionId == v.Key).ToList());

				foreach (KeyValuePair<long, List<SendingQuestion>> pair in qs)
				{
					Thread.Sleep(3500);
					foreach (SendingQuestion q in pair.Value)
					{
						try
						{
							using (var client = new HttpClient())
							{
								var values = new Dictionary<string, string>
							{
								 { "type", "unicast" }, //type - тип запроса. Может быть:self - самому себе, broadcast - вещание по каналу, unicast - отправка одному пользователю
								 { "id", AppSettingConfig.PushallId }, //id - номер вашей подписки в случае с self - ваш айди
								 { "key", AppSettingConfig.PushallKey }, //key - ключ вашей подписки, или ключ безопасности для отправки Push себе
								 { "uid", q.PushAllUserId }, //uid - ID пользователя
								 { "title", q.ProviderDescription }, //title - заголовок Push-уведомления
								 { "text", q.Title }, //text - основной текст Push-уведомления
								 { "url", q.Url }, //url - адрес по которому будет осуществлен переход по клику (не обязательно)
								 { "hidden", "0" }, //hidden - 1 - сразу скрыть уведомление из истории пользователей, 2 - скрыть только из ленты, 0 - не скрывать (по-умолчанию 0)
								 { "encode", "utf8" }, //encode - ваша кодировка. (не обязательно) например cp1251
								 { "priority", "0" }, //priority - приоритет. 0 - по по-умолчанию. -1 - не важные, менее заметны. 1 - более заметные
							};

								var content = new FormUrlEncodedContent(values);

								var response = await client.PostAsync("https://pushall.ru/api.php", content);

								var responseString = await response.Content.ReadAsStringAsync();
							}
						}
						catch (Exception ex)
						{
							_errorProvider.SendError(ex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				_errorProvider.SendError(ex);
			}
		}
	}
}
