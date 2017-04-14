using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Utils
{
	// http://stackoverflow.com/a/22630057
	// Suppressing "warning CS4014"
	public static class TaskExtensions
	{
		[MethodImpl(Met‌​hodImplOptions.AggressiveInlining)]
		public static void Forget(this Task task)
		{
			task.ConfigureAwait(false);
		}
	}
}