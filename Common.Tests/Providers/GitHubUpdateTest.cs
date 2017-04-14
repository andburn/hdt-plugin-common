using System;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Providers.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.Common.Tests
{
	[TestClass]
	public class GitHubUpdateTest
	{
		private static IUpdateService updater;
		private static GithubRelease release;

		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			updater = new GitHubUpdateService();
		}

		[TestInitialize]
		public void TestSetup()
		{
			release = new GithubRelease() {
				Url = string.Empty,
				Tag = "v0.2.0",
				PreRelease = "False",
				Publsihed = string.Empty
			};
		}

		// check a live repo (TODO better to stub this)
		[TestMethod]
		public void LiveRequest()
		{
			var result = updater.CheckForUpdate(
				new Uri("https://api.github.com/repos/andburn/hdt-plugin-endgame/releases"),
				new Version(0, 1, 0)).Result;
			Assert.IsTrue(result.HasUpdate);
		}

		[TestMethod]
		public void RecognizeVersionFromTag_WithPrefix()
		{
			var result = new GitHubUpdateResult(release, new Version(0, 1, 0));
			Assert.IsTrue(result.HasUpdate);
			Assert.AreEqual(new Version(0, 2, 0), result.Version);
		}

		[TestMethod]
		public void RecognizeVersionFromTag_WithoutPrefix()
		{
			release.Tag = "0.1.5";
			var result = new GitHubUpdateResult(release, new Version(0, 1, 0));
			Assert.IsTrue(result.HasUpdate);
			Assert.AreEqual(new Version(0, 1, 5), result.Version);
		}

		[TestMethod]
		public void UpdateNotAvailable_IfVersionIsLower()
		{
			var result = new GitHubUpdateResult(release, new Version(0, 3, 0));
			Assert.IsFalse(result.HasUpdate);
		}

		[TestMethod]
		public void PreRelease_IsFalse()
		{
			var result = new GitHubUpdateResult(release, new Version(0, 1, 0));
			Assert.IsFalse(result.IsPreRelease);
		}

		[TestMethod]
		public void PreRelease_IsTrue()
		{
			release.PreRelease = "True";
			var result = new GitHubUpdateResult(release, new Version(0, 1, 0));
			Assert.IsTrue(result.IsPreRelease);
		}

		[TestMethod]
		public void NoUpdate_IfReleaseIsNull()
		{
			var result = new GitHubUpdateResult(null, new Version(0, 1, 0));
			Assert.IsFalse(result.HasUpdate);
		}

		[TestMethod]
		public void NoUpdate_IfVersionIsNull()
		{
			var result = new GitHubUpdateResult(release, null);
			Assert.IsFalse(result.HasUpdate);
		}
	}
}