using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class ProblemBase : ISolvable
	{
		public string[] Answer { get; set; }

		public int Day => int.Parse(GetType().Namespace.Split('.').Last().Replace("Day", null));
		public string Title => GetType().FullName.Substring("Ujeby.AoC.App.YearYYYY.DayDD.".Length);

		private readonly (int, ConsoleColor)[] _elapsedColors = new (int, ConsoleColor)[]
		{
			(250, ConsoleColor.Red),
			(100, ConsoleColor.Yellow),
			(10, ConsoleColor.Green),
			(0, ConsoleColor.White),
		};

		public int Solve()
		{
			var result = 0;

			var sw = new Stopwatch();
			try
			{
				sw.Start();
				var answer = SolveProblem(ReadInput());
				var elapsed = (int)sw.Elapsed.TotalMilliseconds;

				var title = $"-={{ {Title} }}=-";
				Log.Text($"-={{ ", indent: 4, textColor: ConsoleColor.Gray);
				Log.Text(Title, textColor: ConsoleColor.White);
				Log.Text($" }}=-", textColor: ConsoleColor.Gray);

				var elapsedMsg = $"-={{ {elapsed}ms }}=-";

				var padding = string.Join("", Enumerable.Repeat("-", 60 - title.Length - elapsedMsg.Length));
				Log.Text(padding, textColor: ConsoleColor.DarkGray);

				Log.Text($"-={{ ", textColor: ConsoleColor.Gray);
				var elapsedColor = ConsoleColor.White;
				foreach (var ec in _elapsedColors)
					if (elapsed >= ec.Item1)
					{
						elapsedColor = ec.Item2;
						break;
					}
				Log.Text($"{elapsed}ms", textColor: elapsedColor);
				Log.Text(" }=-", textColor: ConsoleColor.Gray);

				var answers = $"{answer.Item1?.ToString() ?? "?"}, {answer.Item2?.ToString() ?? "?"}";
				var answersMsg = $"-={{ {answers} }}=-";
				padding = string.Join("", Enumerable.Repeat("-", 38 - answersMsg.Length));
				Log.Text(padding, textColor: ConsoleColor.DarkGray);

				Log.Text($"-={{ ", textColor: ConsoleColor.Gray);

				Log.Text(answer.Item1?.ToString() ?? "?", 
					textColor: 
						Answer[0] != null && Answer[0] != answer.Item1 ? ConsoleColor.Red : 
						(answer.Item1 == null ? ConsoleColor.DarkGray : ConsoleColor.White));
				Log.Text(", ", textColor: ConsoleColor.White);
				Log.Text(answer.Item2?.ToString() ?? "?",
					textColor: 
						Answer[1] != null && Answer[1] != answer.Item2 ? ConsoleColor.Red :
						(answer.Item2 == null ? ConsoleColor.DarkGray : ConsoleColor.White));

				Log.Text(" }=-", textColor: ConsoleColor.Gray);

				var stars = "";
#if _DEBUG_SAMPLE
				stars = "NA";
#else
				stars += (Answer[0] != null && Answer[0] == answer.Item1) ? "*" : " ";
				stars += (Answer[1] != null && Answer[1] == answer.Item2) ? "*" : " ";
#endif
				Log.Text("-", textColor: ConsoleColor.DarkGray);
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray);
				Log.Text($"{stars}",
					textColor: stars == "NA" ? ConsoleColor.DarkGray : ConsoleColor.Yellow);
				Log.Text(" }=-", textColor: ConsoleColor.Gray);

				Log.Line();

				if (Answer[0] != null && Answer[0] == answer.Item1)
					result++;

				if (Answer[1] != null && Answer[1] == answer.Item2)
					result++;
			}
			catch (Exception ex)
			{
				Log.Line(ex.ToString());
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
		protected string _inputFilename => Path.Combine(_workingDir, $"Day{Day:d2}", "input.sample.txt");
#else
		protected string _inputFilename => Path.Combine(_workingDir, $"Day{Day:d2}", "input.txt");
#endif

		public string[] ReadInput()
		{
			return File.ReadLines(_inputFilename).ToArray();
		}
	}
}