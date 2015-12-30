using PushAll.Service.Repository.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Repository
{
	class SqlProvider : ISqlProvider
	{
		private readonly string _connectionString;

		public SqlProvider(string connectionStringName)
		{
			if (string.IsNullOrWhiteSpace(connectionStringName))
			{
				throw new ArgumentNullException("connectionStringName");
			}

			ConnectionStringSettings conString = ConfigurationManager.ConnectionStrings[connectionStringName];

			if (conString == null || String.IsNullOrWhiteSpace(conString.ConnectionString))
				throw new ApplicationException(String.Format("Couldn't find Connection string: '{0}'", connectionStringName));

			_connectionString = new SqlConnectionStringBuilder(conString.ConnectionString)
			{
				AsynchronousProcessing = true
			}.ToString();
		}

		public async Task<T> Execute<T>(string spName, SqlParameter[] spParams, ReadHandler<Task<T>> read) where T : class
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				using (SqlCommand command = new SqlCommand
				{
					Connection = connection,
					CommandType = CommandType.StoredProcedure,
					CommandText = spName,
				})
				{
					if (spParams != null)
					{
						command.Parameters.AddRange(spParams);
					}

					if (connection.State != ConnectionState.Open)
					{
						connection.Open();
					}

					T result = null;

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						if (read != null)
						{
							result = await read(reader);
						}
					}

					return result;
				}
			}
		}

	}
}
