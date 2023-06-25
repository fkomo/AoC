using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_20;

[AoCPuzzle(Year = 2016, Day = 20, Answer1 = "32259706", Answer2 = "113", Skip = false)]
public class FirewallRules : PuzzleBase
{
	private const int _from = 0;
	private const int _to = 1;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var denied = input
			.Select(x => new v2i(x.Split('-').Select(n => long.Parse(n)).ToArray()))
			.OrderBy(x => x[_from])
			.ToArray();

		// part1
		v2i allowedRange = new();
		foreach (var deniedRange in denied)
		{
			if (deniedRange[_from] > allowedRange[_from])
				break;

			if (allowedRange[_from] >= deniedRange[_from] && allowedRange[_from] <= deniedRange[_to])
				allowedRange[_from] = deniedRange[_to] + 1;
		}
		var answer1 = allowedRange[_from];

		// part2
		allowedRange[_from] = 0;
		var allowed = new HashSet<long>();
		foreach (var deniedRange in denied)
		{
			if (deniedRange[_from] > allowedRange[_from])
			{
				allowedRange[_to] = deniedRange[_from] - 1;
				for (var i = allowedRange[_from]; i <= allowedRange[_to]; i++)
					allowed.Add(i);

				allowedRange = new(deniedRange[_to] + 1);
			}

			else if (allowedRange[_from] >= deniedRange[_from] && allowedRange[_from] <= deniedRange[_to])
				allowedRange[_from] = deniedRange[_to] + 1;
		}
		for (var i = denied.Max(x => x[_to]) + 1; i <= uint.MaxValue; i++)
			allowed.Add(i);

		var answer2 = allowed.Count;

		return (answer1.ToString(), answer2.ToString());
	}
}