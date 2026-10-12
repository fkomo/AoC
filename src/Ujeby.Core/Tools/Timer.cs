using System.Collections.Concurrent;
using System.Diagnostics;

namespace Ujeby.Tools
{
	public static class Timer
	{
		static readonly ConcurrentDictionary<string, Stopwatch> _stopwatches = [];

		public static Stopwatch Start(string id = "default")
		{
			if (!_stopwatches.TryGetValue(id, out var sw))
			{
				sw = new Stopwatch();
				_stopwatches[id] = sw;
			}

			sw.Start();
			return sw;
		}

		public static double Stop(string id = "default")
		{
			if (!_stopwatches.TryGetValue(id, out var sw))
				throw new ArgumentException(nameof(id));

			var elapsed = sw.Elapsed.TotalMilliseconds;
			Debug.WriteLine($"[{id}]: {elapsed}ms");

			sw.Stop();
			return elapsed;
		}
	}
}
