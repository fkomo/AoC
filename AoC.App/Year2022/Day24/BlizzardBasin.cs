using System.Runtime.Serialization;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day24
{
	public class BlizzardBasin : PuzzleBase
	{
		public class Blizzard
		{
			public char Sign;
			public v2i Start;
			public v2i Direction;
		}

		private static readonly Dictionary<char, v2i> _blizzDir = new()
		{
			{ '>', new(1, 0) },
			{ 'v', new(0, 1) },
			{ '<', new(-1, 0) },
			{ '^', new(0, -1) }
		};

		private static readonly v2i[] _options = new v2i[]
		{
			new(1, 0),
			new(0, 1),
			new(-1, 0),
			new(0, -1),
			new(0, 0)
		};

		protected override (string, string) SolveProblem(string[] input)
		{
			Debug.Line();

			var blizz = ParseBlizzards(input);

			var mapSize = new v2i(input[0].Length, input.Length);
			var blizzMapSize = new v2i(mapSize.X - 2, mapSize.Y - 2);
			Debug.Line($"{blizz.Length} blizzards in {blizzMapSize.X}x{blizzMapSize.Y} ({blizzMapSize.Area()})");

			var start = new v2i(1, 0);
			var end = new v2i(mapSize.X - 2, mapSize.Y - 1);

			// part1
			var elves = new v3i[]
			{
				new(start, 0)
			};
			long t = 0;
			do
			{
				elves = Step(t, blizz, elves, mapSize, end);
				t++;
			}
			while (elves.Length != 1 || elves[0].ToV2i() != end);
			long? answer1 = elves[0].Z;

			// part2
			// end to start
			do
			{
				elves = Step(t, blizz, elves, mapSize, start);
				t++;
			}
			while (elves.Length != 1 || elves[0].ToV2i() != start);
			// and back to end again
			do
			{
				elves = Step(t, blizz, elves, mapSize, end);
				t++;
			}
			while (elves.Length != 1 || elves[0].ToV2i() != end);
			long? answer2 = elves[0].Z;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		public static v3i[] Step(long time, Blizzard[] blizzards, v3i[] elves, v2i mapSize, v2i end)
		{
			var _map = GetMapInTime(time, blizzards, mapSize);

			var newSpawns = new List<v3i>();
			foreach (var e in elves)
			{
				// reached end
				if (e.ToV2i() == end)
					return new[] { e };

				// killed by blizzard
				if (_map[e.Y, e.X] != 0)
					continue;

				// safe spot, spawn more elves in all directions
				foreach (var o in _options)
				{
					var p1 = e.ToV2i() + o;

					// walls or out of bounds
					if (p1.X < 0 || p1.X >= mapSize.X || p1.Y < 0 || p1.Y >= mapSize.Y ||
						_map[p1.Y, p1.X] == '#')
						continue;

					if (newSpawns.Any(s => s.ToV2i() == p1))
						continue;

					// possible new spawn
					newSpawns.Add(new(p1, time + 1));
				}
			}

			Debug.Line($"#{time}: {newSpawns.Count} elves");
			return newSpawns.ToArray();
		}

		public static Blizzard[] ParseBlizzards(string[] input)
		{
			var result = new List<Blizzard>();
			for (var y = 1; y < input.Length - 1; y++)
				for (var x = 1; x < input[0].Length - 1; x++)
				{
					if (input[y][x] == '.')
						continue;

					result.Add(new Blizzard
					{
						Start = new(x, y),
						Sign = input[y][x],
						Direction = _blizzDir[input[y][x]]
					});
				}

			return result.ToArray();
		}

		public static char[,] GetMapInTime(long t, Blizzard[] blizzards, v2i mapSize)
		{
			var result = new char[mapSize.Y, mapSize.X];

			// side walls
			for (var y = 0; y < mapSize.Y; y++)
			{
				result[y, 0] = '#';
				result[y, mapSize.X - 1] = '#';
			}

			// top wall
			for (var x = 2; x < mapSize.X - 1; x++)
				result[0, x] = '#';

			// bottom wall
			for (var x = 1; x < mapSize.X - 2; x++)
				result[mapSize.Y - 1, x] = '#';

			foreach (var blizz in blizzards)
			{
				var pt = (blizz.Start + blizz.Direction * t) % (mapSize - 2);

				if (pt.X <= 0)
					pt.X += mapSize.X - 2;
				
				if (pt.Y <= 0)
					pt.Y += mapSize.Y - 2;

				result[pt.Y, pt.X] = blizz.Sign;
			}

			return result;
		}
	}
}
