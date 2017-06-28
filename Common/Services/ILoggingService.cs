using System;

namespace HDT.Plugins.Common.Services
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

		void SetDumpFileName(string filename);

		void EnableDumpToFile();

		void DisableDumpToFile();
	}
}