using System;
using System.Threading.Tasks;

namespace Spectator.Core.Model
{
	public class Account
	{
		public Task Login (string token)
		{
			return Task.Run (() => {
			});
		}
	}
}