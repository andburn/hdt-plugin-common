﻿using System;
using System.Reflection;

namespace HDT.Plugins.Common.Plugin
{
	public static class PluginAssembly
	{
		public static string Path
		{
			get
			{
				// Based on StackOverflow - http://stackoverflow.com/a/283917/2762059
				// Get the path of the executing plugin assembly
				string path = null;
				try
				{
					// use CodeBase over Location, more reliable
					var codeBase = Assembly.GetExecutingAssembly().CodeBase;
					UriBuilder uri = new UriBuilder(codeBase);
					// remove uri file protocol
					var escaped = Uri.UnescapeDataString(uri.Path);
					path = System.IO.Path.GetDirectoryName(escaped);
				}
				catch (Exception e)
				{
					Providers.ServiceFactory.CreateLoggingService().Error(e);
				}
				return path;
			}
		}
	}
}