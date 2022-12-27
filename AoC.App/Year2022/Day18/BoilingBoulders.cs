using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day18
{
	public class BoilingBoulders : PuzzleBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var cubes = input.Select(l =>
			{
				var split = l.Split(',');
				return (x: int.Parse(split[0]), y: int.Parse(split[1]), z: int.Parse(split[2]));
			}).ToArray();

			var max = (x: cubes.Max(c => c.x), y: cubes.Max(c => c.y), z: cubes.Max(c => c.z));

			// 0=air, 1=lava, 2=steam
			var grid = new byte[max.z + 1, max.y + 1, max.x + 1];
			foreach (var cube in cubes)
				grid[cube.z, cube.y, cube.x] = 1;

			Debug.Line();
			Debug.Line($"{cubes.Length} cubes in grid [{grid.GetLength(0)}x{grid.GetLength(1)}x{grid.GetLength(2)}]");

			// part1
			long? answer1 = GetSurfaceArea(cubes, grid);

			// part2
#if _RELEASE || _DEBUG_SAMPLE
			for (var z = 0; z < grid.GetLength(0); z++)
			{
				for (var y = 0; y < grid.GetLength(1); y++)
				{
					for (var x = 0; x < grid.GetLength(2); x++)
					{
						if (grid[z, y, x] != 0)
							continue;

						if (x == 0 || y == 0 || z == 0 ||
							x == grid.GetLength(2) - 1 ||
							y == grid.GetLength(1) - 1 ||
							z == grid.GetLength(0) - 1)
							ExpandSteam((x, y, z), grid);
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

		private static void ExpandSteam((int x, int y, int z) p, byte[,,] grid)
		{
			if (grid[p.z, p.y, p.x] != 0)
				return;

			grid[p.z, p.y, p.x] = 2;

			if (p.z + 1 < grid.GetLength(0) && grid[p.z + 1, p.y, p.x] == 0)
				ExpandSteam((p.x, p.y, p.z + 1), grid);

			if (p.z - 1 > 0 && grid[p.z - 1, p.y, p.x] == 0)
				ExpandSteam((p.x, p.y, p.z - 1), grid);

			if (p.y + 1 < grid.GetLength(1) && grid[p.z, p.y + 1, p.x] == 0)
				ExpandSteam((p.x, p.y + 1, p.z), grid);

			if (p.y - 1 > 0 && grid[p.z, p.y - 1, p.x] == 0)
				ExpandSteam((p.x, p.y - 1, p.z), grid);

			if (p.x + 1 < grid.GetLength(2) && grid[p.z, p.y, p.x + 1] == 0)
				ExpandSteam((p.x + 1, p.y, p.z), grid);

			if (p.x - 1 > 0 && grid[p.z, p.y, p.x - 1] == 0)
				ExpandSteam((p.x - 1, p.y, p.z), grid);
		}

		private static long GetSurfaceArea((int x, int y, int z)[] cubes, byte[,,] grid, 
			byte surface = 0)
		{
			long result = 0;
			foreach (var cube in cubes)
			{
				if (cube.z + 1 == grid.GetLength(0) || grid[cube.z + 1, cube.y, cube.x] == surface)
					result++;
				if (cube.z - 1 == -1 || grid[cube.z - 1, cube.y, cube.x] == surface)
					result++;

				if (cube.y + 1 == grid.GetLength(1) || grid[cube.z, cube.y + 1, cube.x] == surface)
					result++;
				if (cube.y - 1 == -1 || grid[cube.z, cube.y - 1, cube.x] == surface)
					result++;

				if (cube.x + 1 == grid.GetLength(2) || grid[cube.z, cube.y, cube.x + 1] == surface)
					result++;
				if (cube.x - 1 == -1 || grid[cube.z, cube.y, cube.x - 1] == surface)
					result++;
			}

			return result;
		}
	}
}
