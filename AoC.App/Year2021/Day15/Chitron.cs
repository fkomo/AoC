using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day15
{
	public class Chitron : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			var riskMap = CreateRiskMap(input, input.Length);
			var dist = Dijkstra.Create(riskMap, (0, 0));
			long? answer1 = dist[riskMap.GetLength(0) - 1, riskMap.GetLength(1) - 1];

			// part2
			var riskMap5 = EnlargeRiskMap(riskMap, input.Length, 5);
			dist = Dijkstra.Create(riskMap5, (0, 0));
			long? answer2 = dist[riskMap5.GetLength(0) - 1, riskMap5.GetLength(1) - 1];

			return (answer1?.ToString(), answer2?.ToString());
		}

		public static int[,] CreateRiskMap(string[] input, int size)
		{
			var risk = new int[size, size];
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					risk[y, x] = input[y][x] - '0';

			return risk;
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
	}
}
