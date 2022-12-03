using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class ProblemBase : ISolvable
	{
		public long?[] Solution { get; set; }

		public bool Solve()
		{
			var result = false;

			var title = $"--=[ { GetType().FullName.Substring("Ujeby.AoC.App.".Length) } ]=";
			title += string.Join("", Enumerable.Repeat("-", Debug.LineLength - title.Length));
			Debug.Line(title);

			var sw = new Stopwatch();
			try
			{
				DebugLine();

				sw.Start();

				var answer = SolveProblem(ReadInput());

				var elapsed = (int)sw.Elapsed.TotalMilliseconds;
				DebugLine($"Solved in ~{ elapsed }ms { (elapsed > 250 ? "[>250ms!]" : null) }");
				DebugLine(AnswerMessage(1, answer.Item1, Solution[0]));
				DebugLine(AnswerMessage(2, answer.Item2, Solution[1]));

				result = true;

				if (Solution[0].HasValue && Solution[0].Value != answer.Item1)
					result = false;

				if (Solution[1].HasValue && Solution[1].Value != answer.Item2)
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

			DebugLine();

			return result;
		}

		private string AnswerMessage(int part, long answer, long? solution)
		{
			var answerMessage = $"Part{part} answer = { answer }";

			if (solution.HasValue && solution.Value != answer)
				answerMessage += $" [!= {solution.Value}]";
			
			else if (!solution.HasValue)
				answerMessage += $" [?]";

			return answerMessage;
		}

		protected abstract (long, long) SolveProblem(string[] input);

		protected static void DebugLine(string message = null)
		{
			Debug.Line("  " + message);
		}

		protected string WorkingDir => Path.Combine(Environment.CurrentDirectory, GetType().FullName.Split('.')[3]);

		private string[] ReadInput(string inputName = null)
		{
#if _DEBUG_SAMPLE
			inputName ??= $"input.sample.txt";
#else
			inputName ??= $"input.txt";
#endif
			return File.ReadLines(Path.Combine(WorkingDir, inputName))
				.ToArray();
		}
	}
}