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
				Output.Text($"-={{ ", indent: 4, textColor: ConsoleColor.Gray);
				Output.Text(Title, textColor: ConsoleColor.White);
				Output.Text($" }}=-", textColor: ConsoleColor.Gray);

				var elapsedMsg = $"-={{ {elapsed}ms }}=-";

				var padding = string.Join("", Enumerable.Repeat("-", 60 - title.Length - elapsedMsg.Length));
				Output.Text(padding, textColor: ConsoleColor.DarkGray);

				Output.Text($"-={{ ", textColor: ConsoleColor.Gray);
				var elapsedColor = ConsoleColor.White;
				foreach (var ec in _elapsedColors)
					if (elapsed >= ec.Item1)
					{
						elapsedColor = ec.Item2;
						break;
					}
				Output.Text($"{elapsed}ms", textColor: elapsedColor);
				Output.Text(" }=-", textColor: ConsoleColor.Gray);

				var answers = $"{answer.Item1?.ToString() ?? "?"}, {answer.Item2?.ToString() ?? "?"}";
				var answersMsg = $"-={{ {answers} }}=-";
				padding = string.Join("", Enumerable.Repeat("-", 38 - answersMsg.Length));
				Output.Text(padding, textColor: ConsoleColor.DarkGray);

				Output.Text($"-={{ ", textColor: ConsoleColor.Gray);

				Output.Text(answer.Item1?.ToString() ?? "?", 
					textColor: 
						Answer[0] != null && Answer[0] != answer.Item1 ? ConsoleColor.Red : 
						(answer.Item1 == null ? ConsoleColor.DarkGray : ConsoleColor.White));
				Output.Text(", ", textColor: ConsoleColor.White);
				Output.Text(answer.Item2?.ToString() ?? "?",
					textColor: 
						Answer[1] != null && Answer[1] != answer.Item2 ? ConsoleColor.Red :
						(answer.Item2 == null ? ConsoleColor.DarkGray : ConsoleColor.White));

				Output.Text(" }=-", textColor: ConsoleColor.Gray);

				var stars = "";
#if _DEBUG_SAMPLE
				stars = "NA";
#else
				stars += (Answer[0] != null && Answer[0] == answer.Item1) ? "*" : " ";
				stars += (Answer[1] != null && Answer[1] == answer.Item2) ? "*" : " ";
#endif
				Output.Text("-", textColor: ConsoleColor.DarkGray);
				Output.Text($"-={{ ", textColor: ConsoleColor.Gray);
				Output.Text($"{stars}",
					textColor: stars == "NA" ? ConsoleColor.DarkGray : ConsoleColor.Yellow);
				Output.Text(" }=-", textColor: ConsoleColor.Gray);

				Output.Line();

				if (Answer[0] != null && Answer[0] == answer.Item1)
					result++;

				if (Answer[1] != null && Answer[1] == answer.Item2)
					result++;
			}
			catch (Exception ex)
			{
				Output.Line(ex.ToString());
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

		private string[] ReadInput()
		{
			return File.ReadLines(_inputFilename).ToArray();
		}
	}
}