using HDT.Plugins.Common.Services;

namespace HDT.Plugins.Common.Providers
{
	public static class ServiceFactory
	{
		private static readonly IDataRepository DataRepository;
		private static readonly ILoggingService LoggingService;
		private static readonly IUpdateService UpdateService;
		private static readonly IEventsService EventService;
		private static readonly IGameClientService GameService;
		private static readonly IConfigurationRepository ConfigRepository;

		static ServiceFactory()
		{
			DataRepository = new TrackerDataRepository();
			LoggingService = new TrackerLoggingService();
			UpdateService = new GitHubUpdateService();
			EventService = new TrackerEventsService();
			GameService = new TrackerClientService();
			ConfigRepository = new TrackerConfigRepository();			
		}

		public static IDataRepository CreateDataRepository() => DataRepository;

		public static ILoggingService CreateLoggingService() => LoggingService;

		public static IUpdateService CreateUpdateService() => UpdateService;

		public static IEventsService CreateEventService() => EventService;

		public static IGameClientService CreateGameClientService() => GameService;

		public static IConfigurationRepository CreateConfigRepository() => ConfigRepository;
	}
}