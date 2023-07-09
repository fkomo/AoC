using System.Diagnostics;
using System.Reflection;

namespace Ujeby.AoC.Common
{
	public abstract class PuzzleBase : IPuzzle
	{
		public string Title => GetType().Name;
		public int Day => GetType().GetCustomAttribute<AoCPuzzleAttribute>().Day;
		public int Year => GetType().GetCustomAttribute<AoCPuzzleAttribute>().Year;

		private (string Part1, string Part2) Answer
			=> (Part1: GetType().GetCustomAttribute<AoCPuzzleAttribute>().Answer1,
				Part2: GetType().GetCustomAttribute<AoCPuzzleAttribute>().Answer2);

		private bool _skip;

		public bool Skip
		{
			set { _skip = value; }
			get { return _skip; }
		}

		public PuzzleBase()
		{
			_skip = GetType().GetCustomAttribute<AoCPuzzleAttribute>().Skip;
		}

		protected abstract (string Part1, string Part2) SolvePuzzle(string[] input);

		public int Solve(string inputStorage)
		{
			var result = 0;

#if _DEBUG_SAMPLE
			// skip (sample) debugging of solved puzzles
			if (Answer.Part1 != null && Answer.Part2 != null)
				return result;
#endif

			var sw = new Stopwatch();
			try
			{
				Debug.Indent += 2;
				Log.Indent += 2;

				var input = InputProvider.Read(inputStorage, Year, Day);

				sw.Start();

				(string Part1, string Part2) solution = default;
				if (!Skip)
					solution = SolvePuzzle(input);
				else
					solution = Answer;

				var elapsed = sw.Elapsed.TotalMilliseconds;

				Log.Indent -= 2;

				// title
				var title = $"{{ #{Day:d2} {Title} }}=-";
				Log.Text($"{{ ", textColor: ConsoleColor.Gray);
				Log.Text($"#{Day:d2} {Title}", textColor: ConsoleColor.White, indent: 0);
				Log.Text($" }}=-", textColor: ConsoleColor.Gray, indent: 0);

				var answers = $"-={{ {solution.Part1?.ToString() ?? "?"}, {solution.Part2?.ToString() ?? "?"} }}=-";

				// padding
				var padding = string.Join("", Enumerable.Repeat("-", (AdventOfCode.ConsoleWidth - 23) - title.Length - answers.Length));
				Log.Text(padding, textColor: ConsoleColor.DarkGray, indent: 0);

				// answers
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				Log.Text(solution.Part1?.ToString() ?? "?", textColor: GetAnswerColor(Answer.Part1, solution.Part1), indent: 0);
				Log.Text(", ", textColor: ConsoleColor.White, indent: 0);
				Log.Text(solution.Part2?.ToString() ?? "?", textColor: GetAnswerColor(Answer.Part2, solution.Part2), indent: 0);
				Log.Text(" }=-", textColor: ConsoleColor.Gray, indent: 0);

				// padding
				Log.Text("-", textColor: ConsoleColor.DarkGray, indent: 0);

				// elapsed
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				Log.Text(Skip ? "skip!" : $"{DurationToStringSimple(elapsed),5}",
					textColor: Skip ? ConsoleColor.DarkMagenta : GetElapsedColor(elapsed), indent: 0);
				Log.Text(" }=-", textColor: ConsoleColor.Gray, indent: 0);

				// padding
				Log.Text("-", textColor: ConsoleColor.DarkGray, indent: 0);

				// stars
				var stars =
					((Answer.Part1 != null && Answer.Part1 == solution.Part1) ? "*" : " ") +
					((Answer.Part2 != null && Answer.Part2 == solution.Part2) ? "*" : " ");
				Log.Text($"-={{ ", textColor: ConsoleColor.Gray, indent: 0);
				Log.Text($"{stars}", textColor: GetStarsColor(stars), indent: 0);
				Log.Text(" }", textColor: ConsoleColor.Gray, indent: 0);

				Log.Line();

				if (Answer.Part1 != null && Answer.Part1 == solution.Part1)
					result++;

				if (Answer.Part2 != null && Answer.Part2 == solution.Part2)
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
				(1000, ConsoleColor.DarkRed),
				(250, ConsoleColor.Red),
				(150, ConsoleColor.DarkYellow),
				(100, ConsoleColor.Yellow),
				(50, ConsoleColor.DarkGreen),
				(0, ConsoleColor.Green),
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
			=> right != null && right != calculated ? ConsoleColor.Red :
				(calculated == null ? ConsoleColor.DarkGray : ConsoleColor.White);

		/// <summary></summary>
		/// <param name="duration">duration in ms</param>
		/// <returns></returns>
		private static string DurationToStringSimple(double duration)
		{
			if (duration < 1)
				return $"{(int)(duration * 1000)}us";

			else if (duration < 1000)
				return $"{(int)duration}ms";

			else if (duration < (60 * 1000))
				return $"{(int)(duration / (1000))}s";

			else if (duration < (60 * 60 * 1000))
				return $"{(int)(duration / (60 * 1000))}min";

			return $"{(long)(duration / (60 * 60 * 1000))}h";
		}
	}
}
