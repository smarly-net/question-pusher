using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAll.Service.App_Start
{
	public static class AppSettingConfig
	{
		private static readonly string _pushallId;
		private static readonly string _pushallKey;

		static AppSettingConfig()
		{
			AppSettingsReader appSettingsReader = new AppSettingsReader();

			_pushallId = (string)appSettingsReader.GetValue("PushAllId", typeof(string));
			_pushallKey = (string)appSettingsReader.GetValue("PushAllKey", typeof(string));
		}

		public static string PushallId
		{
			get { return _pushallId; }
		}

		public static string PushallKey
		{
			get { return _pushallKey; }
		}

	}
}
