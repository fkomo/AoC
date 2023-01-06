using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class PuzzleBase : IPuzzle
	{
		public string[] Answer { get; set; }

		public int Day => int.Parse(GetType().Namespace.Split('.').Last().Replace("Day", null));
		public string Title => GetType().FullName.Substring("Ujeby.AoC.App.YearYYYY.DayDD.".Length);

		public int Solve()
		{
			var result = 0;

			var sw = new Stopwatch();
			try
			{
				Debug.Indent += 2;
				Log.Indent += 2;

				sw.Start();
				var answer = SolvePuzzle(ReadInput());
				var elapsed = sw.Elapsed.TotalMilliseconds;

				Log.Indent -= 2;

				// title
				var title = $"{{ #{Day:d2} {Title} }}=-";
				Log.Text($"{{ ", textColor: ConsoleColor.Gray);
				Log.Text($"#{Day:d2} {Title}", textColor: ConsoleColor.White, indent: 0);
				Log.Text($" }}=-", textColor: ConsoleColor.Gray, indent: 0);

				var answers = $"-={{ {answer.Item1?.ToString() ?? "?"}, {answer.Item2?.ToString() ?? "?"} }}=-";

				// padding
				var padding = string.Join("", Enumerable.Repeat("-", (AdventOfCode.ConsoleWidth - 23) - title.Length - answers.Length));
				Log.Text(padding, textColor: ConsoleColor.DarkGray, indent: 0);

				// answers
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				Log.Text(answer.Item1?.ToString() ?? "?", textColor: GetAnswerColor(Answer[0], answer.Item1), indent: 0);
				Log.Text(", ", textColor: ConsoleColor.White, indent: 0);
				Log.Text(answer.Item2?.ToString() ?? "?", textColor: GetAnswerColor(Answer[1], answer.Item2), indent: 0);
				Log.Text(" }=-", textColor: ConsoleColor.Gray, indent: 0);

				// padding
				Log.Text("-", textColor: ConsoleColor.DarkGray, indent: 0);

				// elapsed
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				Log.Text($"{DurationToStringSimple(elapsed),5}", textColor: GetElapsedColor(elapsed), indent: 0);
				Log.Text(" }=-", textColor: ConsoleColor.Gray, indent: 0);

				// padding
				Log.Text("-", textColor: ConsoleColor.DarkGray, indent: 0);

				// stars
				var stars = 
					((Answer[0] != null && Answer[0] == answer.Item1) ? "*" : " ") + 
					((Answer[1] != null && Answer[1] == answer.Item2) ? "*" : " ");
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				Log.Text($"{stars}", textColor: GetStarsColor(stars), indent: 0);
				Log.Text(" }", textColor: ConsoleColor.Gray, indent: 0);

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

		private static ConsoleColor GetStarsColor(string stars)
		{
			return stars?.Contains('*') == true ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
		}

		private static ConsoleColor GetElapsedColor(double elapsed)
		{
			var elapsedColors = new (int, ConsoleColor)[]
			{
				(250, ConsoleColor.Red),
				(100, ConsoleColor.Yellow),
				(10, ConsoleColor.Green),
				(0, ConsoleColor.White),
			};

			var elapsedColor = ConsoleColor.White;
			foreach (var ec in elapsedColors)
				if (elapsed >= ec.Item1)
				{
					elapsedColor = ec.Item2;
					break;
				}

			return elapsedColor;
		}

		private static ConsoleColor GetAnswerColor(string right, string calculated)
		{
			return right != null && right != calculated ? ConsoleColor.Red 
				: (calculated == null ? ConsoleColor.DarkGray : ConsoleColor.White);
		}

		protected abstract (string, string) SolvePuzzle(string[] input);

		protected string _workingDir => Path.Combine(Environment.CurrentDirectory, GetType().FullName.Split('.')[3]);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="duration">duration in ms</param>
		/// <returns></returns>
		private static string DurationToStringSimple(double duration)
		{
			if (duration < 1)
				return $"{(int)(duration * 1000)}us";

			else if (duration < 1000)
				return $"{(int)duration}ms";

			else if (duration < 60 * 60 * 1000)
				return $"{(int)(duration / 1000)}s";

			return $"{(int)(duration / 60 * 60 * 1000)}h";
		}

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
