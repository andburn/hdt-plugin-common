namespace HDT.Plugins.Common.Data.Services
{
	public interface IConfigurationRepository
	{
		object Get(string key);

		void Set(string key, object value);
	}
}