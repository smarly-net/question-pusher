using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Repository.Base
{
	interface IErrorRepository
	{
		Task InsertError(Exception e);
	}
}
