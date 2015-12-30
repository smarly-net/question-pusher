using PushAll.Service.Engine.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushAll.Service.Model;
using PushAll.Service.Provider;
using AngleSharp.Parser.Html;

namespace PushAll.Service.Engine
{
	class StackoverflowParse : IStackoverflowParse
	{
		private IDownloadProvider _downloadProvider;
		public StackoverflowParse(IDownloadProvider downloadProvider)
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

			string html = await _downloadProvider.DownloadPage("http://ru.stackoverflow.com/");
			var document = new HtmlParser().Parse(html);

			var rows = document.QuerySelectorAll("div.question-summary div.summary");

			foreach (var row in rows)
			{
				var titleTag = row.QuerySelector("h3 a");
				string title = titleTag?.TextContent;
				string url = titleTag?.GetAttribute("href");

				if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(url))
				{
					continue;
				}

				var tagTags = row.QuerySelectorAll("div.tags a");

				;

				result.Add(new Question
				{
					Title = title.Trim(),
					Url = "https://ru.stackoverflow.com" + Uri.UnescapeDataString(url.Trim()) ,
					Provider = QuestionProvider.Stackoverflow,
					Tags = tagTags.Select(x => x?.TextContent).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList()
				});
			}

			return result;
		}

		public Task ParseTags(IList<Question> questions)
		{
			throw new NotImplementedException();
		}

		public Task ParseTagImage(IList<Tag> tags)
		{
			throw new NotImplementedException();
		}
	}
}
