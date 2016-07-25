using HDT.Plugins.Common.Services;

namespace HDT.Plugins.Common.Providers
{
	public static class ServiceFactory
	{
		private static readonly IDataRepository DataRepository;
		private static readonly ILoggingService LoggingService;
		private static readonly IUpdateService UpdateService;

		static ServiceFactory()
		{
			DataRepository = new TrackerDataRepository();
			LoggingService = new TrackerLoggingService();
			UpdateService = new GitHubUpdateService();
		}

		public static IDataRepository CreateDataRepository() => DataRepository;

		public static ILoggingService CreateLoggingService() => LoggingService;

		public static IUpdateService CreateUpdateService() => UpdateService;
	}
}