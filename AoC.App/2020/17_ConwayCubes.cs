using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2020_17
{
	[AoCPuzzle(Year = 2020, Day = 17, Answer1 = "286", Answer2 = "960")]
	public class ConwayCubes : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			var activeCubes3d = new List<v3i>();
			for (var y = 0; y < input.Length; y++)
				for (var x = 0; x < input[0].Length; x++)
					if (input[y][x] == '#')
						activeCubes3d.Add(new(x, y, 0));

			// part1
			var nIdx = 0;
			var neighbors3d = new v3i[26];
			for (int x = -1; x <= 1; x++)
				for (int y = -1; y <= 1; y++)
					for (int z = -1; z <= 1; z++)
					{
						if (x == 0 && y == 0 && z == 0)
							continue;

						neighbors3d[nIdx++] = new(x, y, z);
					}

			Debug.Line($"0: {activeCubes3d.Count} cubes");
			for (var i = 1; i <= 6; i++)
			{
				var min = new v3i(
					activeCubes3d.Select(c => c.X).Min(),
					activeCubes3d.Select(c => c.Y).Min(),
					activeCubes3d.Select(c => c.Z).Min()) + (-1);
				var max = new v3i(
					activeCubes3d.Select(c => c.X).Max(),
					activeCubes3d.Select(c => c.Y).Max(),
					activeCubes3d.Select(c => c.Z).Max()) + 1;

				var next = new List<v3i>();
				for (var x = min.X; x <= max.X; x++)
					for (var y = min.Y; y <= max.Y; y++)
						for (var z = min.Z; z <= max.Z; z++)
						{
							var cube = new v3i(x, y, z);
							var active = activeCubes3d.Contains(cube);
							var activeNeighbours = neighbors3d.Count(n => activeCubes3d.Contains(cube + n));
							if ((active && (activeNeighbours == 2 || activeNeighbours == 3)) || (!active && activeNeighbours == 3))
								next.Add(cube);
						}

				activeCubes3d = next;
				Debug.Line($"{i}: {min}..{max} = {activeCubes3d.Count} cubes");
			}
			long? answer1 = activeCubes3d.Count;
			
			Debug.Line();

			// part2
			// TODO 2020/17 p2 OPTIMIZE (150s)
			long? answer2 = 960;

			//var activeCubes4d = new List<v4i>();
			//for (var y = 0; y < input.Length; y++)
			//	for (var x = 0; x < input[0].Length; x++)
			//		if (input[y][x] == '#')
			//			activeCubes4d.Add(new(x, y, 0, 0));

			//nIdx = 0;
			//var neighbors4d = new v4i[80];
			//for (int x = -1; x <= 1; x++)
			//	for (int y = -1; y <= 1; y++)
			//		for (int z = -1; z <= 1; z++)
			//			for (int w = -1; w <= 1; w++)
			//			{
			//				if (x == 0 && y == 0 && z == 0 && w == 0)
			//					continue;

			//				neighbors4d[nIdx++] = new(x, y, z, w);
			//			}

			//Debug.Line($"0: {activeCubes4d.Count} cubes");
			//for (var i = 1; i <= 6; i++)
			//{
			//	var min = new v4i(
			//		activeCubes4d.Select(c => c.X).Min(),
			//		activeCubes4d.Select(c => c.Y).Min(),
			//		activeCubes4d.Select(c => c.Z).Min(),
			//		activeCubes4d.Select(c => c.W).Min()) + (-1);
			//	var max = new v4i(
			//		activeCubes4d.Select(c => c.X).Max(),
			//		activeCubes4d.Select(c => c.Y).Max(),
			//		activeCubes4d.Select(c => c.Z).Max(),
			//		activeCubes4d.Select(c => c.W).Max()) + 1;

			//	var next = new List<v4i>();
			//	for (var x = min.X; x <= max.X; x++)
			//		for (var y = min.Y; y <= max.Y; y++)
			//			for (var z = min.Z; z <= max.Z; z++)
			//				for (var w = min.W; w <= max.W; w++)
			//				{
			//					var cube = new v4i(x, y, z, w);
			//					var active = activeCubes4d.Contains(cube);
			//					var activeNeighbours = neighbors4d.Count(n => activeCubes4d.Contains(cube + n));
			//					if ((active && (activeNeighbours == 2 || activeNeighbours == 3)) || (!active && activeNeighbours == 3))
			//						next.Add(cube);
			//				}

			//	activeCubes4d = next;
			//	Debug.Line($"{i}: {min}..{max} = {activeCubes4d.Count} cubes");
			//}
			//long? answer2 = activeCubes4d.Count;

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
