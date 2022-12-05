using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day04
{
	internal class CampCleanup : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var inputP = input.Select(l => l.Split(',').Select(p =>
			{
				var pair = p.Split('-');
				return (int.Parse(pair[0]), int.Parse(pair[1]));
			}).ToArray()).ToArray();

			// part1
			long answer1 = inputP.Count(p => (p[0].Item2 - p[0].Item1 >= p[1].Item2 - p[1].Item1) ?
				(p[0].Item1 <= p[1].Item1 && p[0].Item2 >= p[1].Item2) : (p[1].Item1 <= p[0].Item1 && p[1].Item2 >= p[0].Item2));

			// part2
			long answer2 = inputP.Count(p =>
				!((p[0].Item1 < p[1].Item1 && p[0].Item2 < p[1].Item1) || (p[1].Item1 < p[0].Item1 && p[1].Item2 < p[0].Item1)));

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
