using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Util
{
	// Suppressing "warning CS4014"
	// http://stackoverflow.com/a/22630057

	internal static class TaskExtensions
	{
		[MethodImpl(Met‌​hodImplOptions.AggressiveInlining)]
		public static void Forget(this Task task)
		{
			task.ConfigureAwait(false);
		}
	}
}