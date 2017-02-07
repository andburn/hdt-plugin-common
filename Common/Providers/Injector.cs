using HDT.Plugins.Common.Services;
using SimpleInjector;

namespace HDT.Plugins.Common.Providers
{
	public class Injector
	{
		private static Injector instance;

		private Injector() { }

		public static Injector Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Injector();
				}
				return instance;
			}
		}

		private Container container;

		public Container Container
		{
			get
			{
				if (container == null)
				{
					container = new Container();
					Register(container);
				}
				return container;
			}
		}

		private void Register(Container ct)
		{
			ct.Register<IDataRepository, TrackerDataRepository>(Lifestyle.Singleton);
			ct.Register<IConfigurationRepository, TrackerConfigRepository>(Lifestyle.Singleton);
			ct.Register<IEventsService, TrackerEventsService>(Lifestyle.Singleton);
			ct.Register<IGameClientService, TrackerClientService>(Lifestyle.Singleton);
			ct.Register<ILoggingService, TrackerLoggingService>(Lifestyle.Singleton);
			ct.Register<IUpdateService, GitHubUpdateService>(Lifestyle.Singleton);
			ct.Verify();
		}
	}
}