using System;

namespace HDT.Plugins.Common.Data.Services
{
	public interface ILoggingService
	{
		void Error(string message);

		void Error(object obj);

		void Error(Exception ex);

		void Info(string message);

		void Info(object obj);

		void Debug(string message);

		void Debug(object obj);
	}
}