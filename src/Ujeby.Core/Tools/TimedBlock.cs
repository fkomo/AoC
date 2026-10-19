using System.Diagnostics;

namespace Ujeby.Tools
{
	public class TimedBlock(string message, Action<string> writeLineAction) : IDisposable
	{
        readonly string _message = message;
        readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        void IDisposable.Dispose()
		{
            _stopwatch.Stop();

            var line = _message;

            if (_stopwatch.Elapsed.TotalSeconds < 1)
                line += $": {(int)_stopwatch.Elapsed.TotalMilliseconds}ms";
            else if (_stopwatch.Elapsed.TotalMinutes < 1)
                line += $": {_stopwatch.Elapsed.TotalSeconds:F3}s";
			else
                line += $": {_stopwatch.Elapsed}";

            writeLineAction(line);

            GC.SuppressFinalize(this);
        }
	}
}
