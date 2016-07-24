using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT.Plugins.Common.Services;

namespace HDT.Plugins.Common.Providers
{
	public static class ServiceFactory
	{
		private static readonly IDataRepository DataRepository;
		private static readonly ILoggingService LoggingService;

		static ServiceFactory()
		{
			DataRepository = new TrackerDataRepository();
			LoggingService = new TrackerLoggingService();
		}

		public static IDataRepository CreateDataRepository() => DataRepository;

		public static ILoggingService CreateLoggingService() => LoggingService;
	}
}
