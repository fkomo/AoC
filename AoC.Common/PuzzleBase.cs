using System.Diagnostics;
using System.Reflection;

namespace Ujeby.AoC.Common
{
	public abstract class PuzzleBase : IPuzzle
	{
		public string Title => GetType().Name;
		public int Day => GetType().GetCustomAttribute<AoCPuzzleAttribute>().Day;
		public int Year => GetType().GetCustomAttribute<AoCPuzzleAttribute>().Year;

		public (string Part1, string Part2) Answer
			=> (Part1: GetType().GetCustomAttribute<AoCPuzzleAttribute>().Answer1,
				Part2: GetType().GetCustomAttribute<AoCPuzzleAttribute>().Answer2);

		bool _skip;

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

		public int Solve(string inputStorage, 
			string inputSuffix = null)
		{
			var result = 0;

			var sw = new Stopwatch();
			try
			{
				var input = InputProvider.Read(inputStorage, Year, Day, suffix: inputSuffix);
				if (input?.Any() != true)
					return 0;

				sw.Start();

				(string Part1, string Part2) solution = default;
				if (!Skip)
					solution = SolvePuzzle(input);
				else
					solution = Answer;

				var elapsed = sw.Elapsed.TotalMilliseconds;

				// day number
				Log.ChristmasPattern("│", 2);
				var title = $"#{Day:d2} {Title}";
				Log.Text($"{Day:d2} ", indent: 2);

				// stars
				var stars =
					((Answer.Part1 != null && Answer.Part1 == solution.Part1) ? "*" : " ") +
					((Answer.Part2 != null && Answer.Part2 == solution.Part2) ? "*" : " ");
				Log.Text($"{stars}", textColor: GetStarsColor(stars), indent: 0);

				// title
				Log.ChristmasText(Title, indent: 1);

				if (solution.Part1 == null && solution.Part2 == null)
					return 0;

				// elapsed
				var elapsedText = Skip ? "skip!" : $"{DurationToStringSimple(elapsed)}";
				Log.Text($"{elapsedText,5}",
					textColor: Skip ? ConsoleColor.DarkMagenta : GetElapsedColor(elapsed),
					indent: AdventOfCode.ConsoleWidth - title.Length - 55);

				// answers
				var answer1Text = solution.Part1?.ToString() ?? "?";
				Log.Text($"{answer1Text}", textColor: GetAnswerColor(Answer.Part1, solution.Part1), indent: 1);
				var answer2Text = solution.Part2?.ToString() ?? "?";
				Log.Text($"{answer2Text}", textColor: GetAnswerColor(Answer.Part2, solution.Part2), indent: 1);

				if (Answer.Part1 != null && Answer.Part1 == solution.Part1)
					result++;

				if (Answer.Part2 != null && Answer.Part2 == solution.Part2)
					result++;
			}
			catch (Exception ex)
			{
				Log.Line();
				Log.Line(ex.ToString(), textColor: ConsoleColor.Red);
			}
			finally
			{
				sw.Stop();
				Log.Line();
			}

			return result;
		}

		static ConsoleColor GetStarsColor(string stars)
			=> stars?.Contains('*') == true ? ConsoleColor.Yellow : ConsoleColor.DarkGray;

		static ConsoleColor GetElapsedColor(double elapsed)
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

		static ConsoleColor GetAnswerColor(string right, string calculated)
			=> right != null && right != calculated ? ConsoleColor.Red :
				(calculated == null ? ConsoleColor.DarkGray : ConsoleColor.DarkGray);

		/// <summary></summary>
		/// <param name="duration">duration in ms</param>
		/// <returns></returns>
		static string DurationToStringSimple(double duration)
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
