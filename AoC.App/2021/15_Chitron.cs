using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_15
{
	[AoCPuzzle(Year = 2021, Day = 15, Answer1 = "811", Answer2 = "3012", Skip = true)]
	public class Chitron : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var riskMap = CreateRiskMap(input, input.Length);
			var end = new v2i(riskMap.GetLength(0) - 1, riskMap.GetLength(1) - 1);

			var dijkstra = new Alg.Dijkstra(riskMap, new(0, 0), end);
			dijkstra.StepFull();

			long? answer1 = dijkstra.Dist[end.Y, end.X];

			// part2
			// TODO 2021/15 OPTIMIZE p2 (1min)
			var riskMap5 = EnlargeRiskMap(riskMap, input.Length, 5);
			end = new v2i(riskMap5.GetLength(0) - 1, riskMap5.GetLength(1) - 1);
			dijkstra = new Alg.Dijkstra(riskMap5, new(0, 0), end);
			dijkstra.StepFull();

			long? answer2 = dijkstra.Dist[end.Y, end.X];

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
