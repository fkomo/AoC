using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_21
{
	[AoCPuzzle(Year = 2015, Day = 21, Answer1 = "111", Answer2 = "188")]
	public class RPGSimulator20XX : PuzzleBase
	{
		public record struct Item(string Name, v3i Properties)
		{
			public override string ToString() => $"{Name} {Properties}";
		}

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// x: hp, y: damage, z: armor
			var boss = new v3i(input[0].ToNumArray()[0], input[1].ToNumArray()[0], input[2].ToNumArray()[0]);

			// x: cost, y: damage, z: armor
			var weapons = new Item[]
			{
				new Item("Dagger", new (8, 4, 0)),
				new Item("Shortsword", new (10, 5, 0)),
				new Item("Warhammer", new (25, 6, 0)),
				new Item("Longsword", new (40, 7, 0)),
				new Item("Greataxe", new (74, 8, 0))
			};

			var armor = new Item[]
			{
				new Item("no-armor", new (0, 0, 0)),

				new Item("Leather", new (13, 0, 1)),
				new Item("Chainmail", new (31, 0, 2)),
				new Item("Splintmail", new (53, 0, 3)),
				new Item("Bandedmail", new (75, 0, 4)),
				new Item("Platemail", new (102, 0, 5))
			};

			var rings = new Item[]
			{
				new Item("no-ring", new (0, 0, 0)),
				new Item("no-ring", new (0, 0, 0)),

				new Item("Damage+1", new (25, 1, 0)),
				new Item("Damage+2", new (50, 2, 0)),
				new Item("Damage+3", new (100, 3, 0)),
				new Item("Defense+1", new (20, 0, 1)),
				new Item("Defense+2", new (40, 0, 2)),
				new Item("Defense+3", new (80, 0, 3))
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
							var player = w.Properties + a.Properties + rings[r1].Properties + rings[r2].Properties;
							var cost = player.X;

							player.X = 100; // hp
							var fight = Fight(player, boss);

							if (cost < answer1 && fight)
							{
								answer1 = cost;
								//Debug.Line($"{w}, {a}, {rings[r1]}, {rings[r2]}");
							}
							else if (cost > answer2 && !fight)
								answer2 = cost;
						}

			return (answer1.ToString(), answer2.ToString());
		}

		private static bool Fight(v3i player, v3i boss)
			=> (player.X / Math.Max(1, boss.Y - player.Z)) >= (boss.X / Math.Max(1, player.Y - boss.Z));
	}
}
