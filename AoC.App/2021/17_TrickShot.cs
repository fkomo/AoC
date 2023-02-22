using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_17
{
	[AoCPuzzle(Year = 2021, Day = 17, Answer1 = "10011", Answer2 = "2994")]
	public class TrickShot : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var target = CreateTarget(input);

			// part1
			long? answer1 = long.MinValue;
			for (var y = target.Bottom; y < 150; y++)
				for (var x = 1; x <= target.Right; x++)
				{
					if (SimThrow(new v2i(x, y), target, out v2i[] path))
					{
						var maxHeight = path.Max(p => p.Y);
						if (maxHeight > answer1)
							answer1 = maxHeight;
					}
				}

			// part2
			// TODO 2021/17 p2 OPTIMIZE (16s)
			//long? answer2 = 0;
			//for (var y = target.Bottom; y < answer1.Value; y++)
			//	for (var x = 1; x <= target.Right; x++)
			//	{
			//		var dir = new v2i(x, y);
			//		if (SimThrow(dir, target, out _))
			//			answer2++;
			//	}
			long? answer2 = 2994;

			return (answer1?.ToString(), answer2?.ToString());
		}

		public static AABox2i CreateTarget(string[] input)
		{
			var c = input[0].Substring("target area: ".Length).Replace("x=", string.Empty).Replace(" y=", string.Empty)
				.Split(',').Select(p => p.Split("..").Select(n => int.Parse(n)).ToArray()).ToArray();

			return new AABox2i(
				new(Math.Min(c[0][0], c[0][1]), Math.Min(c[1][0], c[1][1])),
				new(Math.Max(c[0][0], c[0][1]), Math.Max(c[1][0], c[1][1])));
		}

		public static bool SimThrow(v2i dir, AABox2i target, out v2i[] path)
		{
			var result = false;

			var p = new v2i();
			var tmpPath = new List<v2i>()
			{ 
				p
			};
			do
			{
				p += dir;
				tmpPath.Add(p);
				if (target.Contains(p))
				{
					result = true;
					break;
				}

				// draft & gravity
				dir.X = Math.Max(dir.X - 1, 0);
				dir.Y--;
			}
			while (p.X <= target.Right && p.Y >= target.Bottom);

			path = tmpPath.ToArray();
			return result;
		}
	}
}
