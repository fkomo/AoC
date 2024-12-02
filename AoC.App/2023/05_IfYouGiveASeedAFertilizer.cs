using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_05;

[AoCPuzzle(Year = 2023, Day = 05, Answer1 = "51752125", Answer2 = "12634632", Skip = true)]
public class IfYouGiveASeedAFertilizer : PuzzleBase
{
	const int _dst = 0;
	const int _src = 1;
	const int _len = 2;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var seeds = input.First().ToNumArray();

		var maps = input.Skip(2).ToArray().Split(string.Empty)
			.Select(x => x.Skip(1).Select(xx => new v3i(xx.ToNumArray())).ToArray())
			.ToArray();

		// part1
		var answer1 = seeds.Select(x => Map(x, maps)).Min();

		// part2
		// TODO 2023/05 OPTIMIZE p2 (>1h)
		var locations = new ConcurrentBag<long>();
		Parallel.For(0, seeds.Length / 2, (seedIdx) =>
		{
			var first = seeds[seedIdx * 2];
			var last = seeds[seedIdx * 2] + seeds[seedIdx * 2 + 1];

			var minLocation = long.MaxValue;
			for (var s = first; s < last; s++)
			{
				var location = Map(s, maps);
				if (location < minLocation)
					minLocation = location;
			}

			Log.Line($"[{first}..{last}]>{minLocation}");
			locations.Add(minLocation);
		});
		var answer2 = locations.Min();

		return (answer1.ToString(), answer2.ToString());
	}

	static long Map(long source, v3i[][] maps)
	{
		var mapped = source;
		foreach (var map in maps)
		{
			var range = map.SingleOrDefault(x => x[_src] <= mapped && mapped - x[_src] < x[_len]);
			if (range == default)
				continue;

			mapped = mapped - range[_src] + range[_dst];
		}

		return mapped;
	}
}