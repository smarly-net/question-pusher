using PushAll.Service.Engine.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PushAll.Service.Model;
using System.Net.Http;
using AngleSharp.Parser.Html;
using PushAll.Service.Provider;

namespace PushAll.Service.Engine
{
	class TosterParse : ITosterParse
	{
		private IDownloadProvider _downloadProvider;
		public TosterParse(IDownloadProvider downloadProvider)
		{
			if (downloadProvider == null)
			{
				throw new ArgumentNullException("downloadProvider");
			}
			_downloadProvider = downloadProvider;
		}

		public async Task<IList<Question>> ParseList()
		{
			List<Question> result = new List<Question>();

			string html = await _downloadProvider.DownloadPage("https://toster.ru/questions/latest");
			var document = new HtmlParser().Parse(html);

			var cells = document.QuerySelectorAll("ul.content-list li.content-list__item");

			foreach (var cell in cells)
			{
				var titleTag = cell.QuerySelector("h2 a");
				string title = titleTag?.TextContent;
				string url = titleTag?.GetAttribute("href");

				if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(url))
				{
					continue;
				}

				result.Add(new Question
				{
					Title = title.Trim(),
					Url = Uri.UnescapeDataString(url.Trim()),
					Provider = QuestionProvider.Toster,
				});
			}

			return result;
		}

		public async Task ParseTagImage(IList<Tag> tags)
		{
			foreach (var tag in tags)
			{
				try
				{
					string html = await _downloadProvider.DownloadPage(tag.Url + "/info");
					var document = new HtmlParser().Parse(html);

					string subscribers = document.QuerySelectorAll("header div.mini-counter__count").FirstOrDefault()?.TextContent;

					int subs;
					if (int.TryParse((subscribers ?? string.Empty).Trim(), out subs))
					{
						tag.Subscribers = subs;
					}
					else
					{
						tag.Subscribers = 0;
					}

					string imgUrl = document.QuerySelectorAll("header img.tag__image").FirstOrDefault()?.Attributes["src"]?.Value;

					if (imgUrl != null)
					{
						var handler = new HttpClientHandler();
						using (var client = new HttpClient(handler))
						{
							using (HttpResponseMessage response = await client.GetAsync(imgUrl.Replace("resize_w=120", "resize_w=" + (tag.Subscribers > 10000 ? "30" : "20"))))
							{
								tag.Image = await response.Content.ReadAsByteArrayAsync();
							}
						}
					}

				}
				catch (Exception)
				{
				}
			}
		}

		public async Task ParseTags(IList<Question> questions)
		{
			foreach (var question in questions)
			{
				question.Tags = new List<string>();

				System.Threading.Thread thread = System.Threading.Thread.CurrentThread;
				System.Globalization.CultureInfo originalCulture = thread.CurrentCulture;

				try
				{
					thread.CurrentCulture = new System.Globalization.CultureInfo("ru");

					string html = await _downloadProvider.DownloadPage(question.Url);
					var document = new HtmlParser().Parse(html);

					var cells = document.QuerySelectorAll("#question_show div.question_full nav.question__tags li a");

					foreach (var cell in cells)
					{
						string tagName = cell?.TextContent;

						if (string.IsNullOrWhiteSpace(tagName))
						{
							continue;
						}

						tagName = tagName.Trim();
						question.TagUrls = question.TagUrls != null ? question.TagUrls : new Dictionary<string, string>();

						question.TagUrls[tagName] = cell.Attributes["href"]?.Value;

						if (!question.Tags.Any(x => string.Equals(tagName, x, StringComparison.CurrentCultureIgnoreCase)))
						{
							question.Tags.Add(tagName);
						}
					}
				}
				finally
				{
					thread.CurrentCulture = originalCulture;
				}
			}
		}
	}
}
