using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Repository.Base
{
	public delegate T ReadHandler<T>(SqlDataReader reader);

	interface ISqlProvider
	{
		Task<T> Execute<T>(string spName, SqlParameter[] spParams, ReadHandler<Task<T>> read) where T : class;
	}
}
