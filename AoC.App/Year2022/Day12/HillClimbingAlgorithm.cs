using Ujeby.AoC.App.Year2022.Day09;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day12
{
	public class HillClimbingAlgorithm : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var heightMap = CreateHeightMap(input, out (int x, int y) start, out (int x, int y) end);

			Debug.Line();
			Debug.Line($"width={heightMap.GetLength(1)}, height={heightMap.GetLength(0)}, start={start.y}x{start.x}, end={end.y}x{end.x}");

			// part1
			//Dijkstra(heightMap, start, out char[,] dir, out long[,] dist);
			//var path = GetPathFromEndToStart(start, end, heightMap, dist);
			//long? answer1 = path.Length;

			BreadthFirst(heightMap, start, out (int x, int y)?[,] prev);
			var path = BreadthFirstPath(start, end, prev);
			long? answer1 = path.Length;

			// part2
			long? answer2 = null;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static bool CheckHeight(int high, int low) => high <= low || high == low + 1;

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

		public static void Dijkstra(int[,] map, (int x, int y) start, out char[,] path, out long[,] dist)
		{
			dist = new long[map.GetLength(0), map.GetLength(1)];
			for (var y = 0; y < map.GetLength(0); y++)
				for (var x = 0; x < map.GetLength(1); x++)
					dist[y, x] = long.MaxValue;

			var visited = new bool[map.GetLength(0), map.GetLength(1)];
			path = new char[map.GetLength(0), map.GetLength(1)];

			dist[start.y, start.x] = 0;

			long shortestDist;
			var shortest = start;
			do
			{
				// visit
				foreach (var dir in Directions.NSWE)
				{
					var x1 = dir.Value[0] + shortest.x;
					var y1 = dir.Value[1] + shortest.y;

					// visited or out of bounds
					if (x1 < 0 || y1 < 0 || x1 == map.GetLength(1) || y1 == map.GetLength(0) || visited[y1, x1])
						continue;

					// map points must be connected
					if (!CheckHeight(map[y1, x1], map[shortest.y, shortest.x]))
						continue;

					var r = dist[shortest.y, shortest.x] + map[y1, x1];
					if (r < dist[y1, x1])
					{
						dist[y1, x1] = r;
						path[y1, x1] = Directions.OppositeNSWE[dir.Key];
					}
				}
				visited[shortest.y, shortest.x] = true;

				// find next, shortest and not visited
				shortestDist = long.MaxValue;
				for (var y = 0; y < map.GetLength(0); y++)
					for (var x = 0; x < map.GetLength(1); x++)
					{
						if (visited[y, x])
							continue;

						if (dist[y, x] <= shortestDist)
						{
							shortestDist = dist[y, x];
							shortest = (x, y);
						}
					}
			}
			while (shortestDist != long.MaxValue);
		}
		
		public static (int x, int y)[] DijkstraPath((int x, int y) start, (int x, int y) end, int[,] heightMap, long[,] dist)
		{
			var path = new List<(int x, int y)>();

			var pos = end;
			var bestDist = long.MaxValue;
			do
			{
				(int x, int y)? nextPos = null;
				foreach (var dir in Directions.NSWE)
				{
					var x1 = dir.Value[0] + pos.x;
					var y1 = dir.Value[1] + pos.y;
					if (x1 < 0 || y1 < 0 || x1 == heightMap.GetLength(1) || y1 == heightMap.GetLength(0))
						continue;

					if (path.Any() && path.Last().x == x1 && path.Last().y == y1)
						continue;

					if (!CheckHeight(heightMap[pos.y, pos.x], heightMap[y1, x1]))
						continue;

					if (dist[y1, x1] <= bestDist)
					{
						bestDist = dist[y1, x1];
						nextPos = (x1, y1);
					}
				}

				pos = nextPos.Value;

				path.Add(pos);
			}
			while (pos.x != start.x || pos.y != start.y);

			path.Reverse();
			return path.ToArray();
		}

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

		public static (int x, int y)[] BreadthFirstPath((int x, int y) start, (int x, int y) end, (int x, int y)?[,] prev)
		{
			var path = new List<(int x, int y)>();
			var p = end;
			while (p.x != start.x || p.y != start.y)
			{
				path.Add(p);

				if (!prev[p.y, p.x].HasValue)
					break;

				p = prev[p.y, p.x].Value;
			}

			path.Reverse();
			return path.ToArray();
		}
	}
}
