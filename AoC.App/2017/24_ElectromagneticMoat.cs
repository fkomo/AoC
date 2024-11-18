using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_24;

[AoCPuzzle(Year = 2017, Day = 24, Answer1 = "2006", Answer2 = "1994", Skip = false)]
public class ElectromagneticMoat : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var components = input.Select(x => new v2i(x.ToNumArray())).ToArray();

		// part1
		var answer1 = BuildStrongestBridge(components, []);

		// part2
		var answer2 = BuildLongestAndStrongestBridge(components, [], new v2i(long.MinValue))[_str];

		return (answer1.ToString(), answer2.ToString());
	}

	static long BuildStrongestBridge(v2i[] components, int[] bridge, long port = 0, long strongest = long.MinValue)
	{
		for (var i = 0; i < components.Length; i++)
		{
			if (bridge.Contains(i))
				continue;

			if (components[i].X != port && components[i].Y != port)
				continue;

			var strength = BuildStrongestBridge(components, [.. bridge, i], port: components[i].X == port ? components[i].Y : components[i].X, strongest: strongest);
			strongest = System.Math.Max(strongest, strength);
		}

		return System.Math.Max(strongest, bridge.Sum(x => components[x].X + components[x].Y));
	}

	const int _len = 0;
	const int _str = 1;

	static v2i BuildLongestAndStrongestBridge(v2i[] components, int[] bridge, v2i best, long port = 0)
	{
		for (var i = 0; i < components.Length; i++)
		{
			if (bridge.Contains(i))
				continue;

			if (components[i].X != port && components[i].Y != port)
				continue;

			var b = BuildLongestAndStrongestBridge(components, [.. bridge, i], best, port: components[i].X == port ? components[i].Y : components[i].X);

			if ((b[_len] > best[_len]) || (b[_len] == best[_len] && b[_str] > best[_str]))
				best = b;
		}

		var len = bridge.Length; 
		var str = bridge.Sum(x => components[x].X + components[x].Y);

		if ((len > best[_len]) || (len == best[_len] && str > best[_str]))
			best = new v2i([len, str]);

		return best;
	}
}