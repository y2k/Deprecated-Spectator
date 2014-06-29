using System;

namespace Spectator.Core.Model.Tasks
{
	public class Result<T>
	{
		public T Value { get; set; }

		public Exception Error { get; set; }
	}
}