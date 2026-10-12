using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_13;

[AoCPuzzle(Year = 2017, Day = 13, Answer1 = "2508", Answer2 = "3913186", Skip = false)]
public class PacketScanners : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var layers = input
			.Select(x =>
			{
				var xy = x.ToNumArray();
				return new v3i(xy[0], xy[1], (xy[1] - 1) * 2);
			})
			.ToArray();

		// part1
		var answer1 = layers
			.Where(x => (x[0] % x[2]) == 0)
			.Sum(x => x[0] * x[1]);

		// part2
		var delay = 0;
		while (++delay > 0)
		{
			//if (!layers.Any(x => (x[0] + delay) % (x[2]) == 0))
			//	break;

			var hit = false;
			for (var i = 0; i < layers.Length; i++)
			{
				var layer = layers[i];
				if (((layer[0] + delay) % layer[2]) == 0)
				{
					hit = true;
					break;
				}
			}

			if (!hit)
				break;
		}
		var answer2 = delay;

		return (answer1.ToString(), answer2.ToString());
	}
}