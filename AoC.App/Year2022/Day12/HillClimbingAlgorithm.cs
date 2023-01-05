using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day12
{
	public class HillClimbingAlgorithm : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var heightMap = CreateHeightMap(input, out v2i start, out v2i end);

			// part1
			var bfs = new BreadthFirstSearch(heightMap, start, CheckHeight);
			bfs.StepFull();

			long? answer1 = bfs.Path(end).Length;

			// part2
			long answer2 = long.MaxValue;
			for (var y = 0; y < heightMap.GetLength(0); y++)
			// its enough to check first column, because other possibilities cant climb the hill
			//for (var x = 0; x < heightMap.GetLength(1); x++)
			{
				var x = 0;
				if (heightMap[y, x] != 1)
					continue;

				bfs = new BreadthFirstSearch(heightMap, new(x,y), CheckHeight);
				bfs.StepFull();

				var length = bfs.Path(end)?.Length;
				if (length.HasValue && length.Value < answer2)
					answer2 = length.Value;
			}

			return (answer1?.ToString(), answer2.ToString());
		}

		public static bool CheckHeight(v2i a, v2i b, int[,] weights) 
			=> weights[a.Y, a.X] <= weights[b.Y, b.X] || weights[a.Y, a.X] == weights[b.Y, b.X] + 1;

		public static int[,] CreateHeightMap(string[] input, out v2i start, out v2i end)
		{
			start = new(0, 0);
			end = new(0, 0);

			var map = new int[input.Length, input.First().Length];
			for (var y = 0; y < map.GetLength(0); y++)
				for (var x = 0; x < map.GetLength(1); x++)
				{
					var height = input[y][x] - 'a';

					if (input[y][x] == 'S')
					{
						height = 0;
						start = new(x, y);
					}
					else if (input[y][x] == 'E')
					{
						height = 'z' - 'a';
						end = new(x, y);
					}

					// there can be only 1 zero, start position
					map[y, x] = height + 1;
				}

			return map;
		}
	}
}
