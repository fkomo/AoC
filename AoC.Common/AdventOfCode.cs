namespace Ujeby.AoC.Common
{
	public static class AdventOfCode
	{
		public static void Run(string title, ISolvable[] problemsToSolve)
		{
			var header = $"~~#[ {title} ]#";
			header += string.Join("", Enumerable.Repeat("~", Debug.LineLength - header.Length));
			Debug.Line(header);
			Debug.Line();

			var solved = 0;
			try
			{
				foreach (var problem in problemsToSolve)
					solved += problem.Solve() ? 1 : 0;
			}
			catch (Exception ex)
			{
				Debug.Line(ex.ToString());
			}
			finally
			{
				var result = $"~~#[ Solved { solved }/{ problemsToSolve.Length } problems ]#";
				result += string.Join("", Enumerable.Repeat("~", Debug.LineLength - result.Length));

				Debug.Line(result);
			}
		}
	}
}
