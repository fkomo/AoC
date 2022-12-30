using Ujeby.AoC.App.Year2022.Day09;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day12
{
	public class HillClimbingAlgorithm : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var heightMap = CreateHeightMap(input, out (int x, int y) start, out (int x, int y) end);

			// part1
			var prev = BreadthFirstSearch.Create(heightMap, start, connectionCheck: CheckHeight);
			var path = BreadthFirstSearch.Path(start, end, prev);
			long? answer1 = path.Length;

			// part2
			long answer2 = long.MaxValue;
			for (var y = 0; y < heightMap.GetLength(0); y++)
			// its enough to check first column, because other possibilities cant climb the hill
			//for (var x = 0; x < heightMap.GetLength(1); x++)
			{
				var x = 0;
				if (heightMap[y, x] != 1)
					continue;

				var tmpPrev = BreadthFirstSearch.Create(heightMap, (x, y), connectionCheck: CheckHeight);
				var length = BreadthFirstSearch.Path((x, y), end, tmpPrev)?.Length;
				if (length.HasValue && length.Value < answer2)
					answer2 = length.Value;
			}

			return (answer1?.ToString(), answer2.ToString());
		}

		public static bool CheckHeight((int x, int y) n1, (int x, int y) n2, int[,] weights) 
			=> weights[n1.y, n1.x] <= weights[n2.y, n2.x] || weights[n1.y, n1.x] == weights[n2.y, n2.x] + 1;

		public static int[,] CreateHeightMap(string[] input, out (int x, int y) start, out (int x, int y) end)
		{
			start = (0, 0);
			end = (0, 0);

			var map = new int[input.Length, input.First().Length];
			for (var y = 0; y < map.GetLength(0); y++)
				for (var x = 0; x < map.GetLength(1); x++)
				{
					var height = input[y][x] - 'a';

					if (input[y][x] == 'S')
					{
						height = 0;
						start = (x, y);
					}
					else if (input[y][x] == 'E')
					{
						height = 'z' - 'a';
						end = (x, y);
					}

					// there can be only 1 zero, start position
					map[y, x] = height + 1;
				}

			return map;
		}
	}
}
