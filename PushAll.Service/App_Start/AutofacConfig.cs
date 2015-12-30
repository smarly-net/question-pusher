using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PushAll.Service.Services;
using PushAll.Service.Engine.Base;
using PushAll.Service.Engine;
using PushAll.Service.Repository.Base;
using PushAll.Service.Repository;
using PushAll.Service.Provider;

namespace PushAll.Service.App_Start
{
	internal static class AutofacConfig
	{
		internal static IContainer Init()
		{
			var builder = new ContainerBuilder();

			Register(builder);

			IContainer container = builder.Build();

			return container;
		}

		private static void Register(ContainerBuilder builder)
		{
			builder.RegisterType<SqlProvider>().As<ISqlProvider>()
				.WithParameter("connectionStringName", "DatabaseConnection")
				.SingleInstance();

			builder.RegisterType<DownloadProvider>().As<IDownloadProvider>().SingleInstance();

			builder.RegisterType<PushAllProvider>().As<IPushAllProvider>().SingleInstance();

			builder.RegisterType<ErrorProvider>().As<IErrorProvider>().SingleInstance();
			builder.RegisterType<ErrorRepository>().As<IErrorRepository>().SingleInstance();

			builder.RegisterType<TosterParseService>().SingleInstance();
			builder.RegisterType<StackoverflowParseService>().SingleInstance();

			builder.RegisterType<TosterParse>().As<ITosterParse>().SingleInstance();
			builder.RegisterType<StackoverflowParse>().As<IStackoverflowParse>().SingleInstance();

			builder.RegisterType<QuestionRepository>().As<IQuestionRepository>().SingleInstance();

		}
	}
}
