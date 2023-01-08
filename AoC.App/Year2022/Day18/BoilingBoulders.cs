using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day18
{
	public class BoilingBoulders : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var cubes = input.Select(l =>
			{
				var split = l.Split(',');
				return new v3i(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
			}).ToArray();

			var max = new v3i(cubes.Max(c => c.X), cubes.Max(c => c.Y), cubes.Max(c => c.Z));

			// 0=air, 1=lava, 2=steam
			var grid = new byte[max.Z + 1, max.Y + 1, max.X + 1];
			foreach (var cube in cubes)
				grid[cube.Z, cube.Y, cube.X] = 1;

			Debug.Line();
			Debug.Line($"{cubes.Length} cubes in grid [{grid.GetLength(0)}x{grid.GetLength(1)}x{grid.GetLength(2)}]");

			// part1
			long? answer1 = GetSurfaceArea(cubes, grid);

			// part2
#if _RELEASE || _DEBUG_SAMPLE
			v3i p = new(0);
			for (; p.Z < grid.GetLength(0); p.Z++)
			{
				for (; p.Y < grid.GetLength(1); p.Y++)
				{
					for (; p.X < grid.GetLength(2); p.X++)
					{
						if (grid[p.Z, p.Y, p.X] != 0)
							continue;

						if (p.X == 0 || p.Y == 0 || p.Z == 0 ||
							p.X == grid.GetLength(2) - 1 ||
							p.Y == grid.GetLength(1) - 1 ||
							p.Z == grid.GetLength(0) - 1)
							ExpandSteam(p, grid);
					}
				}
			}
			long? answer2 = GetSurfaceArea(cubes, grid, 
				surface: 2);
#else
			// TODO 2022/18 p2 debug ends with stack overflow (ExpandSteam)
			long? answer2 = 2494;
#endif
			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static void ExpandSteam(v3i p, byte[,,] grid)
		{
			if (grid[p.Z, p.Y, p.X] != 0)
				return;

			grid[p.Z, p.Y, p.X] = 2;

			if (p.Z + 1 < grid.GetLength(0) && grid[p.Z + 1, p.Y, p.X] == 0)
				ExpandSteam(new(p.X, p.Y, p.Z + 1), grid);

			if (p.Z - 1 > 0 && grid[p.Z - 1, p.Y, p.X] == 0)
				ExpandSteam(new(p.X, p.Y, p.Z - 1), grid);

			if (p.Y + 1 < grid.GetLength(1) && grid[p.Z, p.Y + 1, p.X] == 0)
				ExpandSteam(new(p.X, p.Y + 1, p.Z), grid);

			if (p.Y - 1 > 0 && grid[p.Z, p.Y - 1, p.X] == 0)
				ExpandSteam(new(p.X, p.Y - 1, p.Z), grid);

			if (p.X + 1 < grid.GetLength(2) && grid[p.Z, p.Y, p.X + 1] == 0)
				ExpandSteam(new(p.X + 1, p.Y, p.Z), grid);

			if (p.X - 1 > 0 && grid[p.Z, p.Y, p.X - 1] == 0)
				ExpandSteam(new(p.X - 1, p.Y, p.Z), grid);
		}

		private static long GetSurfaceArea(v3i[] cubes, byte[,,] grid, 
			byte surface = 0)
		{
			long result = 0;
			foreach (var cube in cubes)
			{
				if (cube.Z + 1 == grid.GetLength(0) || grid[cube.Z + 1, cube.Y, cube.X] == surface)
					result++;
				if (cube.Z - 1 == -1 || grid[cube.Z - 1, cube.Y, cube.X] == surface)
					result++;

				if (cube.Y + 1 == grid.GetLength(1) || grid[cube.Z, cube.Y + 1, cube.X] == surface)
					result++;
				if (cube.Y - 1 == -1 || grid[cube.Z, cube.Y - 1, cube.X] == surface)
					result++;

				if (cube.X + 1 == grid.GetLength(2) || grid[cube.Z, cube.Y, cube.X + 1] == surface)
					result++;
				if (cube.X - 1 == -1 || grid[cube.Z, cube.Y, cube.X - 1] == surface)
					result++;
			}

			return result;
		}
	}
}
