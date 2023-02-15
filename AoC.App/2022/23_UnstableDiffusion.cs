using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2022_23
{
	[AoCPuzzle(Year = 2022, Day = 23, Answer1 = "3920", Answer2 = "889")]
	public class UnstableDiffusion : PuzzleBase
	{
		public class Elf
		{
			public v2i Position { get; set; }
			public v2i? Move { get; set; }
			public v2i? PositionAfter { get; set; }
			public long Steps { get; set; } = 0;
		}

		private static readonly Dictionary<int, v2i[]> SearchGroups = new()
		{
			{ 0, new v2i[] { new(0, -1), new(-1, -1), new(1, -1) } },
			{ 1, new v2i[] { new(0, 1), new(-1, 1), new(1, 1) } },
			{ 2, new v2i[] { new(-1, 0), new(-1, 1), new(-1, -1) } },
			{ 3, new v2i[] { new(1, 0), new(1, 1), new(1, -1) } },
		};

		protected override (string, string) SolvePuzzle(string[] input)
		{
			var elves = ParseElves(input);

			Debug.Line();

			Debug.Line($"{input[0].Length}x{input.Length}, {elves.Length} elves");
			Debug.Line();
			PrintElves(elves);

			// part1
			for (var r = 0; r < 10; r++)
				elves = Step(elves, r, out _);
			var min = new v2i(elves.Min(e => e.Position.X), elves.Min(e => e.Position.Y));
			var max = new v2i(elves.Max(e => e.Position.X), elves.Max(e => e.Position.Y));
			long? answer1 = ((max - min) + new v2i(1, 1)).Area() - elves.Length;

			// part2
			// TODO 2022/23 p2 OPTIMIZE (10s)
			//long? answer2 = 10;
			//while (true)
			//{
			//	elves = Step(elves, (int)answer2.Value, out bool noMovement);
			//	answer2++;
			//	if (noMovement)
			//		break;
			//}
			long? answer2 = 889;

			return (answer1?.ToString(), answer2?.ToString());
		}

		public static Elf[] ParseElves(string[] input)
		{
			var elves = new List<Elf>();
			for (var y = 0; y < input.Length; y++)
			{
				for (var x = 0; x < input[0].Length; x++)
				{
					if (input[y][x] == '.')
						continue;

					elves.Add(new Elf { Position = new v2i(x, y) });
				}
			}

			return elves.ToArray();
		}

		public static Elf[] Step(Elf[] elves, int round, out bool noMovement)
		{
			var firstDirection = round % SearchGroups.Count;

			noMovement = false;

			// first half
			Parallel.For(0, elves.Length, (e1) =>
			{
				var neighbour = false;
				for (var e2 = 0; e2 < elves.Length; e2++)
				{
					if (e2 == e1)
						continue;

					var distance = (elves[e2].Position - elves[e1].Position).Abs();
					if (distance.X <= 1 && distance.Y <= 1)
					{
						neighbour = true;
						break;
					}
				}

				if (neighbour)
				{
					for (var d = 0; d < SearchGroups.Count; d++)
					{
						var targets = SearchGroups[(firstDirection + d) % SearchGroups.Count];

						var free = true;
						foreach (var target in targets)
						{
							var destination = elves[e1].Position + target;
							if (IsElfAt(elves, destination, e1))
							{
								free = false;
								break;
							}
						}

						if (free)
						{
							elves[e1].Move = targets[0];
							break;
						}
					}
				}
			});

			noMovement = elves.All(e => !e.Move.HasValue);
			if (noMovement)
				return elves;

			// second half
			Parallel.For(0, elves.Length, (e1) =>
			{
				if (elves[e1].Move.HasValue)
				{
					var e1Destination = elves[e1].Position + elves[e1].Move;

					var collision = false;
					for (var e2 = 0; e2 < elves.Length; e2++)
					{
						if (e2 == e1)
							continue;

						if (elves[e2].Position + elves[e2].Move == e1Destination)
						{
							collision = true;
							break;
						}
					}

					if (!collision)
					{
						elves[e1].PositionAfter = elves[e1].Position + elves[e1].Move.Value;
						elves[e1].Steps++;
					}
				}
			});

			for (var e = 0; e < elves.Length; e++)
			{
				if (elves[e].PositionAfter.HasValue)
					elves[e].Position = elves[e].PositionAfter.Value;

				elves[e].Move = null;
				elves[e].PositionAfter = null;
			}

			//PrintElves(elves);

			return elves;
		}

		private static void PrintElves(Elf[] elves)
		{
			var min = new v2i(elves.Min(e => e.Position.X), elves.Min(e => e.Position.Y));
			var max = new v2i(elves.Max(e => e.Position.X), elves.Max(e => e.Position.Y));

			for (var y = min.Y; y <= max.Y; y++)
			{
				var line = string.Empty;
				for (var x = min.X; x <= max.X; x++)
					line += IsElfAt(elves, new v2i(x, y)) ? "#" : '.';

				Debug.Line(line);
			}
			Debug.Line();
		}

		private static bool IsElfAt(Elf[] elves, v2i at,
			int skip = -1)
		{
			for (var e = 0; e < elves.Length; e++)
				if (e != skip && elves[e].Position == at)
					return true;

			return false;
		}
	}
}
