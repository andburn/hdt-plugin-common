namespace HDT.Plugins.Common.Services
{
	public interface IConfigurationRepository
	{
		object Get(string key);

		void Set(string key, object value);
	}
}