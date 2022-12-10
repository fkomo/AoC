using Ujeby.AoC.App.Year2022.Day09;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day15
{
	public class Chitron : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			var riskMap = CreateRiskMap(input, input.Length);
			var path = LowestRiskPath(riskMap);
			long? answer1 = path[path.GetLength(0) - 1, path.GetLength(1) - 1];

			// part2
			var riskMap5 = EnlargeRiskMap(riskMap, input.Length, 5);
			var path5 = LowestRiskPath(riskMap5);
			long? answer2 = path5[path5.GetLength(0) - 1, path5.GetLength(1) - 1];
			// TODO 2021/15 part2 still wrong (3019) although answer for sample is correct

			return (answer1?.ToString(), answer2?.ToString());
		}

		public static int[,] EnlargeRiskMap(int[,] originalRiskMap, int size, int scale)
		{
			var riskMap = new int[size * scale, size * scale];
			for (var y = 0; y < size * scale; y++)
				for (var x = 0; x < size * scale; x++)
				{
					if (y < size && x < size)
					{
						riskMap[y, x] = originalRiskMap[y, x];
						continue;
					}

					var x0 = x - size;
					if (x0 < 0)
						x0 = x;

					var y0 = y - size;
					if (y0 < 0)
						y0 = y;
					var r = riskMap[y0, x0];

					if (x >= size)
						r++;
					if (y >= size)
						r++;

					if (r >= 10)
						r -= 9;

					riskMap[y, x] = r;
				}

			return riskMap;
		}

		public static int[,] CreateRiskMap(string[] input, int size)
		{
			var risk = new int[size, size];
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					risk[y, x] = input[y][x] - '0';

			return risk;
		}

		public static long[,] LowestRiskPath(int[,] risk)
		{
			var size = risk.GetLength(0);

			var path = new long[size, size];
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					path[y, x] = long.MaxValue;

			path[0, 0] = 0;
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					Visit(risk, path, x, y);

			return path;
		}

		public static void Visit(int[,] risk, long[,] path, int x, int y)
		{
			var size = risk.GetLength(0);
			foreach (var dir in Directions.NSWE.Values)
			{
				var x1 = dir[0] + x;
				var y1 = dir[1] + y;
				if (x1 < 0 || y1 < 0 || x1 == size || y1 == size)
					continue;

				var r = path[y, x] + risk[y1, x1];
				if (r < path[y1, x1])
					path[y1, x1] = r;
			}
		}
	}
}
