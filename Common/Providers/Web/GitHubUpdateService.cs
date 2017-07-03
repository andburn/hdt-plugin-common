using HDT.Plugins.Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Providers.Web
{
	public class GitHubUpdateService : IUpdateService
	{
		// Query the GitHub API to get the latest release for a repo
		public async Task<IUpdateResult> CheckForUpdate(string name, string location, Version version)
		{
			var source = new Uri($"https://api.github.com/repos/{name}/{location}/releases");
			var json = string.Empty;

			using (WebClient wc = new WebClient())
			{
				Common.Log.Debug($"GitHub: Checking {source}");
				// API requires user-agent string, user name or app name preferred
				wc.Headers.Add(HttpRequestHeader.UserAgent, name);
				json = await wc.DownloadStringTaskAsync(source);
			}
			var releases = JsonConvert.DeserializeObject<List<GithubRelease>>(json);
			Common.Log.Debug($"GitHub: Release {releases.FirstOrDefault()}");
			return new GitHubUpdateResult(releases.FirstOrDefault(), version);
		}
	}

	public class GitHubUpdateResult : IUpdateResult
	{
		// semver regex allowing for leading 'v'
		private static readonly Regex _versionRegex =
			new Regex(@"v?(?<semver>((\d+\.)+\d+))", RegexOptions.Compiled);

		private GithubRelease _release;
		private Version _currentVersion;

		public string DownloadUrl { get; private set; }

		public bool HasUpdate { get; private set; }

		public bool IsPreRelease { get; private set; }

		public Version Version { get; private set; }

		public GitHubUpdateResult(GithubRelease githubRelease, Version version)
		{
			_release = githubRelease;
			_currentVersion = version;
			if (githubRelease == null || version == null)
				HasUpdate = false;
			else
				ProcessRelease();
		}

		private void ProcessRelease()
		{
			// check if tag is a valid version string
			var match = _versionRegex.Match(_release.Tag);
			if (match.Success)
			{
				Version v = new Version(match.Groups["semver"].Value);
				// check if latest version is newer than the current
				if (v.CompareTo(_currentVersion) > 0)
				{
					HasUpdate = true;
					Version = v;
					DownloadUrl = _release.Url;
					IsPreRelease = _release.PreRelease == "True" ? true : false;
					Common.Log.Debug($"GitHub: release {v} is new");
				}
				else
				{
					HasUpdate = false;
					Common.Log.Debug($"GitHub: release {v} is old");
				}
			}
		}
	}

	// Basic release info for JSON deserialization
	public class GithubRelease
	{
		[JsonProperty("html_url")]
		public string Url { get; set; }

		[JsonProperty("tag_name")]
		public string Tag { get; set; }

		[JsonProperty("prerelease")]
		public string PreRelease { get; set; }

		[JsonProperty("published_at")]
		public string Published { get; set; }

		public override string ToString()
		{
			return $"{Tag} ({Published})";
		}
	}
}