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
				Debug.Indent += 2;

				sw.Start();
				var answer = SolveProblem(ReadInput());
				var elapsed = sw.Elapsed.TotalMilliseconds;

				var title = $"-={{ #{Day:d2} {Title} }}=-";
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray);
				Log.Text($"#{Day:d2} {Title}", textColor: ConsoleColor.White, indent: 0);
				Log.Text($" }}=-", textColor: ConsoleColor.Gray, indent: 0);

				var elapsedMsg = $"-={{ {DurationToString(elapsed)} }}=-";

				var padding = string.Join("", Enumerable.Repeat("-", 50 - title.Length - elapsedMsg.Length));
				Log.Text(padding, textColor: ConsoleColor.DarkGray, indent: 0);

				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				var elapsedColor = ConsoleColor.White;
				foreach (var ec in _elapsedColors)
					if (elapsed >= ec.Item1)
					{
						elapsedColor = ec.Item2;
						break;
					}
				Log.Text($"{DurationToString(elapsed)}", textColor: elapsedColor, indent: 0);
				Log.Text(" }=-", textColor: ConsoleColor.Gray, indent: 0);

				var answers = $"{answer.Item1?.ToString() ?? "?"}, {answer.Item2?.ToString() ?? "?"}";
				var answersMsg = $"-={{ {answers} }}=-";
				padding = string.Join("", Enumerable.Repeat("-", 45 - answersMsg.Length));
				Log.Text(padding, textColor: ConsoleColor.DarkGray, indent: 0);

				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);

				Log.Text(answer.Item1?.ToString() ?? "?",
					textColor:
						Answer[0] != null && Answer[0] != answer.Item1 ? ConsoleColor.Red :
						(answer.Item1 == null ? ConsoleColor.DarkGray : ConsoleColor.White), indent: 0);
				Log.Text(", ", textColor: ConsoleColor.White, indent: 0);
				Log.Text(answer.Item2?.ToString() ?? "?",
					textColor:
						Answer[1] != null && Answer[1] != answer.Item2 ? ConsoleColor.Red :
						(answer.Item2 == null ? ConsoleColor.DarkGray : ConsoleColor.White), indent: 0);

				Log.Text(" }=-", textColor: ConsoleColor.Gray, indent: 0);

				var stars = "";
#if _DEBUG_SAMPLE
				stars = "NA";
#else
				stars += (Answer[0] != null && Answer[0] == answer.Item1) ? "*" : " ";
				stars += (Answer[1] != null && Answer[1] == answer.Item2) ? "*" : " ";
#endif
				Log.Text("-", textColor: ConsoleColor.DarkGray, indent: 0);
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				Log.Text($"{stars}",
					textColor: stars == "NA" ? ConsoleColor.DarkGray : ConsoleColor.Yellow, indent: 0);
				Log.Text(" }=-", textColor: ConsoleColor.Gray, indent: 0);

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

				Debug.Indent -= 2;
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="duration">duration in us</param>
		/// <returns></returns>
		private string DurationToString(double duration)
		{
			if (duration < 1)
				return $"{(int)(duration * 1000)}us";
			
			else if (duration < 1000)
				return $"{(int)duration}ms";
			
			else if (duration < 60 * 1000)
				return $">{(int)(duration / 1000)}s";
			
			else if (duration < 60 * 60 * 1000)
				return $">{(int)(duration / (60 * 1000))}min";

			return $">1h";
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