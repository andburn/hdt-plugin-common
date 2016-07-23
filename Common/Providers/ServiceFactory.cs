using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT.Plugins.Common.Services;

namespace HDT.Plugins.Common.Providers
{
	public static class ServiceFactory
	{
		public static IDataRepository CreateDataRepository()
		{
			return new TrackerDataRepository();
		}
	}
}
