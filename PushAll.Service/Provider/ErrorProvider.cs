using PushAll.Service.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.Provider
{
	class ErrorProvider : IErrorProvider
	{
		private IErrorRepository _repository;

		public ErrorProvider(IErrorRepository repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			_repository = repository;
		}

		public void SendError(Exception ex)
		{
			try
			{
				_repository.InsertError(ex);
			}
			catch (Exception)
			{
			}
		}
	}
}
