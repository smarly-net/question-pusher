using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Provider
{
	interface IErrorProvider
	{
		void SendError(Exception ex);
	}
}
