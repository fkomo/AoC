using Ujeby.AoC.App.Year2022.Day09;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day12
{
	public class HillClimbingAlgorithm : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var heightMap = CreateHeightMap(input, out (int x, int y) start, out (int x, int y) end);

			//Dijkstra(heightMap, start, out char[,] dir, out long[,] dist);
			//var path = GetPathFromEndToStart(start, end, heightMap, dist);
			//long? answer1 = path.Length;

			// part1
			BreadthFirst(heightMap, start, out (int x, int y)?[,] prev);
			var path = BreadthFirstPath(start, end, prev);
			long? answer1 = path.Length;

			// part2
			long answer2 = long.MaxValue;
			for (var y = 0; y < heightMap.GetLength(0); y++)
				for (var x = 0; x < heightMap.GetLength(1); x++)
				{
					if (heightMap[y, x] != 1)
						continue;

					BreadthFirst(heightMap, (x, y), out (int x, int y)?[,] tmpPrev);
					var length = BreadthFirstPath((x, y), end, tmpPrev)?.Length;
					if (length.HasValue && length.Value < answer2)
						answer2 = length.Value;
				}

			return (answer1?.ToString(), answer2.ToString());
		}

		public static bool CheckHeight(int high, int low) => high <= low || high == low + 1;

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

		/// <summary>
		/// TODO BFS make common method for later use
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="prev"></param>
		public static void BreadthFirst(int[,] map, (int x, int y) start, out (int x, int y)?[,] prev)
		{
			var visited = new bool[map.GetLength(0), map.GetLength(1)];
			var queue = new Queue<(int x, int y)>();

			prev = new (int x, int y)?[map.GetLength(0), map.GetLength(1)];

			visited[start.y, start.x] = true;

			var p = start;
			do
			{
				if (queue.Any())
					p = queue.Dequeue();

				foreach (var dir in Directions.NSWE)
				{
					var x1 = dir.Value[0] + p.x;
					var y1 = dir.Value[1] + p.y;

					// visited or out of bounds
					if (x1 < 0 || y1 < 0 || x1 == map.GetLength(1) || y1 == map.GetLength(0) || visited[y1, x1])
						continue;

					// map points must be connected
					if (!CheckHeight(map[y1, x1], map[p.y, p.x]))
						continue;

					visited[y1, x1] = true;
					queue.Enqueue((x1, y1));
					prev[y1, x1] = p;
				}
			}
			while (queue.Count > 0);
		}

		/// <summary>
		/// TODO BFS make common method for later use
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="prev"></param>
		public static (int x, int y)[] BreadthFirstPath((int x, int y) start, (int x, int y) end, (int x, int y)?[,] prev)
		{
			var path = new List<(int x, int y)>();
			var p = end;
			while (p.x != start.x || p.y != start.y)
			{
				path.Add(p);

				if (!prev[p.y, p.x].HasValue)
					return null;

				p = prev[p.y, p.x].Value;
			}

			path.Reverse();
			return path.ToArray();
		}
	}
}
