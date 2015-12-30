using PushAll.Service.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushAll.Service.Model;
using System.Data.SqlClient;
using System.Data;

namespace PushAll.Service.Repository
{
	class QuestionRepository : IQuestionRepository
	{
		private ISqlProvider _sqlProvider;

		public QuestionRepository(ISqlProvider sqlProvider)
		{
			if (sqlProvider == null)
			{
				throw new ArgumentNullException("sqlProvider");
			}
			_sqlProvider = sqlProvider;
		}

		public async Task<IList<Question>> AddQuestions(IList<Question> questions)
		{
			var questionsTable = new DataTable();

			questionsTable.Columns.Add("Title", typeof(string));
			questionsTable.Columns.Add("Url", typeof(string));
			questionsTable.Columns.Add("ProviderId", typeof(short));

			foreach (var q in questions)
			{
				questionsTable.Rows.Add(
					q.Title,
					q.Url,
					(short)q.Provider
				);
			}

			return await _sqlProvider.Execute<IList<Question>>(
				"[dbo].[InsertQuestions]",
				new[]
				{
							new SqlParameter("Questions", questionsTable),
				},
				async (reader) =>
				{
					List<Question> result = new List<Question>();

					int questionIdOrdinal = reader.GetOrdinal("QuestionId");
					int titleOrdinal = reader.GetOrdinal("Title");
					int urlOrdinal = reader.GetOrdinal("Url");
					int providerOrdinal = reader.GetOrdinal("ProviderId");

					while (await reader.ReadAsync())
					{
						result.Add(new Question
						{
							QuestionId = reader.GetInt64(questionIdOrdinal),
							Title = reader.GetString(titleOrdinal),
							Url = reader.GetString(urlOrdinal),
							Provider = (QuestionProvider)reader.GetInt16(providerOrdinal),
						});
					}
					return result;
				}
			);
		}

		public async Task<IList<SendingQuestion>> GetQuestionForSend()
		{
			return await _sqlProvider.Execute<IList<SendingQuestion>>(
				"[dbo].[GetQuestionForSend]",
				new SqlParameter[]
				{
				},
				async (reader) =>
				{
					List<SendingQuestion> result = new List<SendingQuestion>();

					int questionIdOrdinal = reader.GetOrdinal("QuestionId");
					int titleOrdinal = reader.GetOrdinal("Title");
					int urlOrdinal = reader.GetOrdinal("Url");
					int providerDescriptionOrdinal = reader.GetOrdinal("ProviderDescription");
					int pushAllUserIdOrdinal = reader.GetOrdinal("PushAllUserId");

					while (await reader.ReadAsync())
					{
						result.Add(new SendingQuestion
						{
							QuestionId = reader.GetInt64(questionIdOrdinal),
							Title = reader.GetString(titleOrdinal),
							Url = reader.GetString(urlOrdinal),
							ProviderDescription = reader.GetString(providerDescriptionOrdinal),
							PushAllUserId = reader.GetString(pushAllUserIdOrdinal),
						});
					}
					return result;
				}
			);
		}

		public async Task UpdateTagImage(IList<Tag> tags)
		{
			var tagTable = new DataTable();

			tagTable.Columns.Add("TagId", typeof(long));
			tagTable.Columns.Add("Image", typeof(byte[]));
			tagTable.Columns.Add("Subscribers", typeof(int));


			foreach (var t in tags)
			{
				tagTable.Rows.Add(
					t.TagId,
					t.Image,
					t.Subscribers
				);
			}

			await _sqlProvider.Execute<object>(
				"[dbo].[UpdateTagImages]",
				new[]
				{
					new SqlParameter("TagImages", tagTable),
				},
				null
			);
		}

		public async Task<IList<Tag>> UpdateTagsAndStatus(IList<Question> questions)
		{
			var tagTable = new DataTable();

			tagTable.Columns.Add("QuestionId", typeof(long));
			tagTable.Columns.Add("TagName", typeof(string));


			foreach (var q in questions)
			{
				foreach (var t in q.Tags)
				{
					tagTable.Rows.Add(
						q.QuestionId,
						t
					);
				}
			}

			return await _sqlProvider.Execute<IList<Tag>>(
				"[dbo].[UpdateTagsAndStatus]",
				new[]
				{
					new SqlParameter("Tags", tagTable),
				},
				async (reader) =>
				{
					List<Tag> result = new List<Tag>();

					int tagIdOrdinal = reader.GetOrdinal("TagId");
					int nameOrdinal = reader.GetOrdinal("Name");

					while (await reader.ReadAsync())
					{
						result.Add(new Tag
						{
							TagId = reader.GetInt32(tagIdOrdinal),
							Name = reader.GetString(nameOrdinal),
						});
					}
					return result;
				}
			);
		}
	}
}
