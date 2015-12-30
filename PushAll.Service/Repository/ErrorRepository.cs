using PushAll.Service.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Repository
{
	class ErrorRepository : IErrorRepository
	{
		private ISqlProvider _sqlProvider;

		public ErrorRepository(ISqlProvider sqlProvider)
		{
			if (sqlProvider == null)
			{
				throw new ArgumentNullException("sqlProvider");
			}
			_sqlProvider = sqlProvider;
		}

		public async Task InsertError(Exception e)
		{
			await _sqlProvider.Execute<object>(
				"[dbo].[InsertError]",
				new[]
				{
					new SqlParameter("Message", e.Message),
					new SqlParameter("StackTrace", e.StackTrace),
				},
				null
			);
		}
	}
}
