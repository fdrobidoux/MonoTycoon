using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTycoon
{
	/// <summary>
	/// Always accesible timing apparatus
	/// </summary>
	public static class StopWatch
	{
		private static Stack<DateTime> timers;

		static StopWatch()
		{
			timers = new Stack<DateTime>(0);
		}


		/// <summary>
		/// Pushes the current DateTime onto the stack of timers
		/// </summary>
		public static void Push()
		{
			timers.Push(DateTime.Now);
		}

		/// <summary>
		/// Pops the top DateTime from the stack of timers,
		/// returns the TimeSpan between when the last Push and now
		/// </summary>        
		public static TimeSpan Pop()
		{
			if (timers.Count > 0)
			{
				DateTime start = timers.Pop();
				return DateTime.Now - start;
			}
			else
			{
				throw new Exception("Tried to pop from the StopWatch while the stack is empty");
			}
		}

		/// <summary>
		/// Calls Pop(), then prints the text &quot;StopWatch: {totalSeconds} \n&quot;
		/// directly to console.
		/// </summary>
		public static void PopAndPrint()
		{
			Console.WriteLine(String.Format("StopWatch: {0:T}", Pop().TotalSeconds));
		}

		/// <summary>
		/// Returns the TimeSpan between the last Push and now
		/// without popping the TimeSpan from the stack of timers
		/// </summary>        
		public static TimeSpan Peek()
		{
			if (timers.Count > 0)
			{
				DateTime start = timers.Peek();
				return DateTime.Now - start;
			}
			else
			{
				throw new Exception("Tried to peek in the StopWatch while the stack is empty");
			}
		}
	}
}
