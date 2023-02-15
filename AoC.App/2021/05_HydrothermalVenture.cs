using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_05
{
	[AoCPuzzle(Year = 2021, Day = 05, Answer1 = "7085", Answer2 = "20271")]
	public class HydrothermalVenture : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var pairs = input
				.Select(line =>
				{
					var n = line.ToNumArray();
					return new v2i[] { new(n[0], n[1]), new(n[2], n[3]) };
				}).ToArray();

			var size = new v2i(
				pairs.Select(pair => Math.Max(pair[0].X, pair[1].X)).Max(), 
				pairs.Select(pair => Math.Max(pair[0].Y, pair[1].Y)).Max()) + 1;

			var map = new int[size.X * size.Y];

			// part1
			foreach (var pair in pairs)
			{
				// vertical
				if (pair[0].X == pair[1].X)
				{
					var length = Math.Abs(pair[0].Y - pair[1].Y) + 1;

					var y = pair[0].Y;
					var dy = (pair[0].Y < pair[1].Y) ? 1 : -1;
					for (var i = 0; i < length; i++, y += dy)
						map[y * size.X + pair[0].X]++;
				}
				// horizontal
				else if (pair[0].Y == pair[1].Y)
				{
					var length = Math.Abs(pair[0].X - pair[1].X) + 1;

					var x = pair[0].X;
					var dx = (pair[0].X < pair[1].X) ? 1 : -1;
					for (var i = 0; i < length; i++, x += dx)
						map[pair[0].Y * size.X + x]++;
				}
			}
			long answer1 = map.Count(p => p > 1);
			
			// part2
			foreach (var pair in pairs)
			{
				// diagonal
				if (Math.Abs(pair[0].X - pair[1].X) == Math.Abs(pair[0].Y - pair[1].Y))
				{
					var length = Math.Abs(pair[0].X - pair[1].X) + 1;

					var dx = (pair[0].X < pair[1].X) ? 1 : -1;
					var dy = (pair[0].Y < pair[1].Y) ? 1 : -1;

					var x = pair[0].X;
					var y = pair[0].Y;
					for (var i = 0; i < length; i++, x += dx, y += dy)
						map[y * size.X + x]++;
				}
			}
			long answer2 = map.Count(p => p > 1);

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
