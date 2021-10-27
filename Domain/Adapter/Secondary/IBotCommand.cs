using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Adapter.Secondary
{
	public interface IBotCommand
	{
		Task ReplyAsync(string message);
	}
}
