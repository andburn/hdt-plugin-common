using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HDT.Plugins.Common.Services;
using Newtonsoft.Json;

namespace HDT.Plugins.Common.Providers
{
	public class GitHubUpdateService : IUpdateService
	{
		// matches github api releases url, with a group on user
		private static readonly Regex _githubRegex =
			new Regex(@"https://api.github.com/repos/(?<user>([^\/]+))/[^\/]+/releases", RegexOptions.Compiled);

		// Query the GitHub API to get the latest release for a repo
		public async Task<IUpdateResult> CheckForUpdate(Uri source, Version version)
		{
			var json = string.Empty;
			var user = GetUsernameFromUrl(source.ToString());

			// throw exception if regex fails on URL
			if (string.IsNullOrEmpty(user))
				throw new FormatException($"The GitHub URL is not valid ({source})");

			using (WebClient wc = new WebClient())
			{
				// API requires user-agent string, user name or app name preferred
				wc.Headers.Add(HttpRequestHeader.UserAgent, user);
				json = await wc.DownloadStringTaskAsync(source);
			}
			var releases = JsonConvert.DeserializeObject<List<GithubRelease>>(json);
			return new GitHubUpdateResult(releases.FirstOrDefault(), version);
		}

		private string GetUsernameFromUrl(string url)
		{
			var match = _githubRegex.Match(url);
			if (match.Success)
			{
				return match.Groups["user"].Value;
			}
			return string.Empty;
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
				}
				else
				{
					HasUpdate = false;
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
		public string Publsihed { get; set; }
	}
}