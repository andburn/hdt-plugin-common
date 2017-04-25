using System.Threading.Tasks;

namespace HDT.Plugins.Common.Services
{
	public interface IHttpClient
	{
		Task<string> JsonGet(string url);

		Task<string> JsonPost(string url, string data);

		Task<string> Get(string url);

		Task<string> Post(string url, string data);
	}
}