using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			var title = $"~~#[ Advent Of Code 2022 ]#";
			title += string.Join("", Enumerable.Repeat("~", Debug.LineLength - title.Length));
			Debug.Line(title);
			Debug.Line();

			var toSolve = new ISolvable[]
			{
				new Day01.Sample() { Solution1 = null, Solution2 = null },

				// TODO 2022
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
