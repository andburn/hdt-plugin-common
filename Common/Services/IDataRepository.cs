using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT.Plugins.Common.Models;

namespace HDT.Plugins.Common.Services
{
	public interface IDataRepository
	{
		List<Deck> GetAllDecks();
	}
}
