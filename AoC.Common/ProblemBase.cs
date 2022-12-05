using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class ProblemBase : ISolvable
	{
		public string[] Answer { get; set; }

		public int Day => int.Parse(GetType().Namespace.Split('.').Last().Replace("Day", null));
		public string Title => GetType().FullName.Substring("Ujeby.AoC.App.".Length);

		public int Solve(
			string inputUrl = null, string session = null)
		{
			var result = 0;

			var sw = new Stopwatch();
			try
			{
				sw.Start();
				var answer = SolveProblem(ReadInput(inputUrl, session));
				var elapsed = (int)sw.Elapsed.TotalMilliseconds;

				var title = $"-=[ {Title} ]=";
				var elapsedMsg = $"=[ {elapsed}ms ]=-";
				var msg = title + string.Join("", Enumerable.Repeat("-", 50 - title.Length - elapsedMsg.Length)) + $"=[ ";
				Debug.Text(msg, indent: 2);
				Debug.Text($"{elapsed}ms",
					textColor: (elapsed > 250 ? ConsoleColor.Red : ConsoleColor.White));
				Debug.Text(" ]=-");
				msg += $"{elapsed}ms ]=-";

				var msg2 = $"=[ { answer.Item1?.ToString() ?? "?" }, {answer.Item2?.ToString() ?? "?"} ]=-";
				var msg3 = string.Join("", Enumerable.Repeat("-", 90 - msg.Length - msg2.Length)) + msg2;

#if _DEBUG_SAMPLE
				var a1 = "N";
				var a2 = "A";
#else
				var a1 = Answer[0] == null ? "" : 
					(Answer[0] != null && Answer[0] == answer.Item1 ? "*" : "");
				var a2 = Answer[1] == null ? "" : 
					(Answer[1] != null && Answer[1] == answer.Item2 ? "*" : "");
#endif

				Debug.Text(msg3 + $"=[ ");
				Debug.Text($"{a1,1}{a2,1}", 
					textColor: ConsoleColor.Yellow);
				Debug.Text(" ]=-");

				Debug.Line();

				if (Answer[0] != null && Answer[0] == answer.Item1)
					result++;

				if (Answer[1] != null && Answer[1] == answer.Item2)
					result++;
			}
			catch (Exception ex)
			{
				Debug.Line(ex.ToString());
			}
			finally
			{
				sw.Stop();
			}

			return result;
		}

		protected abstract (string, string) SolveProblem(string[] input);

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
				Debug.Line($"Input downloaded from {inputUrl}");

				if (!Directory.Exists(_workingDir))
					Directory.CreateDirectory(_workingDir);

				File.WriteAllText(_inputFilename, input);
			}
#endif
			return File.ReadLines(_inputFilename).ToArray();
		}
	}
}