using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class ProblemBase : ISolvable
	{
		public long?[] Answer { get; set; }

		public int Day => int.Parse(GetType().Namespace.Split('.').Last().Replace("Day", null));
		public string Title => GetType().FullName.Substring("Ujeby.AoC.App.".Length);

		public bool Solve(
			string inputUrl = null, string session = null)
		{
			var result = false;

			var sw = new Stopwatch();
			try
			{
				sw.Start();
				var answer = SolveProblem(ReadInput(inputUrl, session));
				var elapsed = (int)sw.Elapsed.TotalMilliseconds;

				var title = $"-=[ {Title} ]=";
				var elapsedMsg = $"=[ {elapsed}ms ]=-";
				var msg = title + string.Join("", Enumerable.Repeat("-", 50 - title.Length - elapsedMsg.Length)) + elapsedMsg;

				var msg2 = $"=[ { answer.Item1?.ToString() ?? "?" }, {answer.Item2?.ToString() ?? "?"} ]=-";
				var msg3 = msg + string.Join("", Enumerable.Repeat("-", 90 - msg.Length - msg2.Length)) + msg2;

#if _DEBUG_SAMPLE
				var a1 = "N";
				var a2 = "A";
#else
				var a1 = !Answer[0].HasValue ? "?" : 
					(Answer[0].HasValue && Answer[0].Value == answer.Item1 ? "*" : "");
				var a2 = !Answer[1].HasValue ? "?" : 
					(Answer[1].HasValue && Answer[1].Value == answer.Item2 ? "*" : "");
#endif
				DebugLine(msg3 + $"=[ {a1,1}{a2,1} ]=-");

				result = true;

				if (!Answer[0].HasValue || Answer[0].Value != answer.Item1)
					result = false;

				if (!Answer[1].HasValue || Answer[1].Value != answer.Item2)
					result = false;
			}
			catch (Exception ex)
			{
				DebugLine(ex.ToString());
			}
			finally
			{
				sw.Stop();
			}

			return result;
		}

		protected abstract (long?, long?) SolveProblem(string[] input);

		protected static void DebugLine(string message = null)
		{
			Debug.Line("  " + message);
		}

		protected string _workingDir => Path.Combine(Environment.CurrentDirectory, GetType().FullName.Split('.')[3]);

#if _DEBUG_SAMPLE
		protected string _inputFilename => Path.Combine(_workingDir, "input.sample.txt");
#else
		protected string _inputFilename => Path.Combine(_workingDir, "input.txt");
#endif

		private string[] ReadInput(string inputUrl, string session)
		{
#if _DEBUG_SAMPLE
#else
			// download input
			if (!File.Exists(_inputFilename) && session != null)
			{
				var httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");
				var input = httpClient.GetStringAsync(inputUrl).Result;
				DebugLine($"Input downloaded from {inputUrl}");

				if (!Directory.Exists(_workingDir))
					Directory.CreateDirectory(_workingDir);

				File.WriteAllText(_inputFilename, input);
			}
#endif
			return File.ReadLines(_inputFilename).ToArray();
		}
	}
}