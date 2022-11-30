using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			var title = $"~~#[ Advent Of Code 2021 ]#";
			title += string.Join("", Enumerable.Repeat("~", Debug.LineLength - title.Length));
			Debug.Line(title);
			Debug.Line();

			var toSolve = new ISolvable[]
			{
				//new Day00.Sample() { Solution1 = null, Solution2 = null },

				new Day01.SonarSweep() { Solution1 = 1226, Solution2 = 1252 },
				new Day02.Dive() { Solution1 = 1868935, Solution2 = 1965970888 },
				new Day03.BinaryDiagnostic() { Solution1 = 2250414, Solution2 = null },

				// TODO 2021
			};

			var solved = 0;
			try
			{
				foreach (var problem in toSolve)
					solved += problem.Solve() ? 1 : 0;
			}
			catch (Exception ex)
			{
				Debug.Line(ex.ToString());
			}
			finally
			{
				var result = $"~~#[ Solved { solved }/{ toSolve.Length } problems ]#";
				result += string.Join("", Enumerable.Repeat("~", Debug.LineLength - result.Length));

				Debug.Line(result);
			}
		}
	}
}
