using Autofac;
using PushAll.Service.App_Start;
using PushAll.Service.Services;
using System;
using System.ComponentModel;
using System.ServiceProcess;
using IContainer = System.ComponentModel.IContainer;

namespace PushAll.Service
{
	class Program
	{
		static void Main(string[] args)
		{
			ServiceBase.Run(new CommonService());

			//new CommonService().StartDebug();

			//Console.WriteLine("Press any key...");
			//Console.ReadKey();
		}
	}

	#region CommonService

	public class CommonService : ServiceBase
	{
		private readonly IContainer components = null;

		public CommonService()
		{
			components = new Container();
		}

		#region IDisposable

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion


		private Autofac.IContainer _iocContainer;

		protected override void OnStart(string[] args)
		{
			base.OnStart(args);

			_iocContainer = AutofacConfig.Init();

			IStartService[] servicesToRun =
			{
				_iocContainer.Resolve<TosterParseService>(),
				_iocContainer.Resolve<StackoverflowParseService>(),
			};

			foreach (var serviceBase in servicesToRun)
			{
				serviceBase.Start(args);
			}
		}

		public void StartDebug()
		{
			OnStart(null);
		}

		protected override void OnStop()
		{
			base.OnStop();
			_iocContainer.Dispose();
		}
	}

	#endregion

}
