using System;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Services
{
	public interface IUpdateService
	{
		Task<IUpdateResult> CheckForUpdate(string name, string location, Version version);
	}

	public interface IUpdateResult
	{
		Version Version { get; }
		string DownloadUrl { get; }
		bool HasUpdate { get; }
		bool IsPreRelease { get; }
	}
}