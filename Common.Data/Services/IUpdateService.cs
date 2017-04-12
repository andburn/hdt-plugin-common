using System;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Data.Services
{
	public interface IUpdateService
	{
		Task<IUpdateResult> CheckForUpdate(Uri source, Version version);
	}

	public interface IUpdateResult
	{
		Version Version { get; }
		string DownloadUrl { get; }
		bool HasUpdate { get; }
		bool IsPreRelease { get; }
	}
}