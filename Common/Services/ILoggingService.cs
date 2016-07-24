using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.Common.Services
{
	public interface ILoggingService
	{
		void Error(string message);

		void Error(object obj);

		void Info(string message);

		void Info(object obj);

		void Debug(string message);

		void Debug(object obj);
	}
}
