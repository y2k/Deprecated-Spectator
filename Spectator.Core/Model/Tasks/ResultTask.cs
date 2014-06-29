using System;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Tasks
{
	public class ResultTask <T> : Task<Result<T>>
	{
		public ResultTask(Func<T> func) : base(()=>{
			try {
				return new Result<T> { Value = func() };
			} catch (Exception e) {
				return new Result<T> { Error = e };
			}
		}) { }
	}

	public class ResultTask : Task<Result<object>>
	{
		public ResultTask(Action func) : base(()=>{
			try {
				func();
				return new Result<object>();
			} catch (Exception e) {
				return new Result<object> { Error = e };
			}
		}) { }

		public new static ResultTask Run(Action func) {
			var t = new ResultTask (func);
			t.Start ();
			return t;
		}

		public new static ResultTask<T> Run<T>(Func<T> func) {
			var t = new ResultTask<T> (func);
			t.Start ();
			return t;
		}
	}
}