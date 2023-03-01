using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_21
{
	[AoCPuzzle(Year = 2015, Day = 21, Answer1 = "111", Answer2 = "188")]
	public class RPGSimulator20XX : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// x: hp, y: damage, z: armor
			var boss = new v3i(input[0].ToNumArray()[0], input[1].ToNumArray()[0], input[2].ToNumArray()[0]);

			// x: cost, y: damage, z: armor
			var weapons = new v3i[]
			{
				new (8, 4, 0),
				new (10, 5, 0),
				new (25, 6, 0),
				new (40, 7, 0),
				new (74, 8, 0)
			};

			var armor = new v3i[]
			{
				new (0, 0, 0),

				new (13, 0, 1),
				new (31, 0, 2),
				new (53, 0, 3),
				new (75, 0, 4),
				new (102, 0, 5)
			};

			var rings = new v3i[]
			{
				new (0, 0, 0),
				new (0, 0, 0),

				new (25, 1, 0),
				new (50, 2, 0),
				new (100, 3, 0),
				new (20, 0, 1),
				new (40, 0, 2),
				new (80, 0, 3)
			};

			// part1
			// part2
			var answer1 = long.MaxValue;
			var answer2 = long.MinValue;

			foreach (var w in weapons)
				foreach (var a in armor)
					for (var r1 = 0; r1 < rings.Length; r1++)
						for (var r2 = r1 + 1; r2 < rings.Length; r2++)
						{
							// add player items together
							var player = w + a + rings[r1] + rings[r2];
							var cost = player.X;

							player.X = 100; // hp
							var fight = Fight(player, boss);

							if (cost < answer1 && fight)
								answer1 = cost;
							else if (cost > answer2 && !fight)
								answer2 = cost;
						}

			return (answer1.ToString(), answer2.ToString());
		}

		private static bool Fight(v3i player, v3i boss)
			=> (player.X / Math.Max(1, boss.Y - player.Z)) >= (boss.X / Math.Max(1, player.Y - boss.Z));
	}
}
