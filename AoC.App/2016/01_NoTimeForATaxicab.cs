using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_01
{
	[AoCPuzzle(Year = 2016, Day = 01, Answer1 = "239", Answer2 = "141")]
	public class NoTimeForATaxicab : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var instr = input.Single().Split(", ").Select(x => new v2i(x[0] == 'R' ? 1 : -1, long.Parse(x[1..]))).ToArray();

			// part1
			var answer1 = new v2i();
			var dir = 3; // Up, v2i.RightDownLeftUp
			foreach (var i in instr)
			{
				dir += v2i.RightDownLeftUp.Length + (int)i.X;
				dir %= v2i.RightDownLeftUp.Length;
				
				answer1 += v2i.RightDownLeftUp[dir] * i.Y;
			}

			// part2
			var answer2 = new v2i();
			dir = 3; // Up, v2i.RightDownLeftUp
			var locations = new List<v2i>()
			{
				new v2i()
			};
			foreach (var i in instr)
			{
				dir += v2i.RightDownLeftUp.Length + (int)i.X;
				dir %= v2i.RightDownLeftUp.Length;

				var sameLocation = false;
				for (var j = 0; j < i.Y; j++)
				{
					answer2 += v2i.RightDownLeftUp[dir];
					if (locations.Contains(answer2))
					{
						sameLocation = true;
						break;
					}
					locations.Add(answer2);
				}

				if (sameLocation)
					break;
			}

			return (answer1.ManhLength().ToString(), answer2.ManhLength().ToString());
		}
	}
}
