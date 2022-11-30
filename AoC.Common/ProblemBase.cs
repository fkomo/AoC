using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class ProblemBase : ISolvable
	{
		public long? Solution1 { get; set; }
		public long? Solution2 { get; set; }

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

				var answer = SolveProblem();

				DebugLine($"Solved in { sw.Elapsed.TotalMilliseconds }ms");
				DebugLine($"Part1 answer = { answer.Item1 }({(Solution1.HasValue ? Solution1.Value.ToString() : "?")})");
				DebugLine($"Part2 answer = { answer.Item2 }({(Solution2.HasValue ? Solution2.Value.ToString() : "?")})");

				result = true;

				if (Solution1.HasValue && Solution1.Value != answer.Item1)
					result = false;

				if (Solution2.HasValue && Solution2.Value != answer.Item2)
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

		protected abstract (long, long) SolveProblem();

		public static void DebugLine(string message = null)
		{
			Debug.Line("  " + message);
		}

		protected string WorkingDir => Path.Combine(Environment.CurrentDirectory, GetType().FullName.Split('.')[3]);

		protected string[] ReadInputLines(string inputName = "input")
		{
			return File.ReadLines(Path.Combine(WorkingDir, $"{inputName}.txt"))
				.ToArray();
		}
	}
}