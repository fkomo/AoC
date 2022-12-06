using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class ProblemBase : ISolvable
	{
		public string[] Answer { get; set; }

		public int Day => int.Parse(GetType().Namespace.Split('.').Last().Replace("Day", null));
		public string Title => GetType().FullName.Substring("Ujeby.AoC.App.".Length);

		private readonly (int, ConsoleColor)[] _elapsedColors = new (int, ConsoleColor)[]
		{
			(250, ConsoleColor.Red),
			(100, ConsoleColor.Yellow),
			(10, ConsoleColor.Green),
			(0, ConsoleColor.White),
		};

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

				var title = $"-={{ {Title} }}=-";
				Debug.Text($"-={{ ", indent: 2, textColor: ConsoleColor.Gray);
				Debug.Text(Title, textColor: ConsoleColor.White);
				Debug.Text($" }}=-", textColor: ConsoleColor.Gray);

				var elapsedMsg = $"-={{ {elapsed}ms }}=-";

				var padding = string.Join("", Enumerable.Repeat("-", 50 - title.Length - elapsedMsg.Length));
				Debug.Text(padding, textColor: ConsoleColor.DarkGray);

				Debug.Text($"-={{ ", textColor: ConsoleColor.Gray);
				var elapsedColor = ConsoleColor.White;
				foreach (var ec in _elapsedColors)
					if (elapsed >= ec.Item1)
					{
						elapsedColor = ec.Item2;
						break;
					}
				Debug.Text($"{elapsed}ms", textColor: elapsedColor);
				Debug.Text(" }=-", textColor: ConsoleColor.Gray);

				var answers = $"{answer.Item1?.ToString() ?? "?"}, {answer.Item2?.ToString() ?? "?"}";
				var answersMsg = $"-={{ {answers} }}=-";
				padding = string.Join("", Enumerable.Repeat("-", 38 - answersMsg.Length));
				Debug.Text(padding, textColor: ConsoleColor.DarkGray);

				Debug.Text($"-={{ ", textColor: ConsoleColor.Gray);
				Debug.Text(answers);
				Debug.Text(" }=-", textColor: ConsoleColor.Gray);

				var stars = "";
#if _DEBUG_SAMPLE
				stars = "NA";
#else
				stars += (Answer[0] != null && Answer[0] == answer.Item1) ? "*" : " ";
				stars += (Answer[1] != null && Answer[1] == answer.Item2) ? "*" : " ";
#endif
				Debug.Text("-", textColor: ConsoleColor.DarkGray);
				Debug.Text($"-={{ ", textColor: ConsoleColor.Gray);
				Debug.Text($"{stars}",
					textColor: stars == "NA" ? ConsoleColor.DarkGray : ConsoleColor.Yellow);
				Debug.Text(" }=-", textColor: ConsoleColor.Gray);

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