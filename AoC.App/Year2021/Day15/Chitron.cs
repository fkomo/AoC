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
			Dijkstra(riskMap, (0,0), out long[,] dist);
			long? answer1 = dist[riskMap.GetLength(0) - 1, riskMap.GetLength(1) - 1];

			// part2
			var riskMap5 = EnlargeRiskMap(riskMap, input.Length, 5);
			Dijkstra(riskMap5, (0,0), out dist);
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

		/// <summary>
		/// TODO dijkstra make unified common method for later use
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="dist"></param>
		/// <param name="nodesConnected"></param>
		public static void Dijkstra(int[,] map, (int x, int y) start, out long[,] dist, 
			Func<int, int, bool> nodesConnected = null)
		{
			dist = new long[map.GetLength(0), map.GetLength(1)];
			for (var y = 0; y < map.GetLength(0); y++)
				for (var x = 0; x < map.GetLength(1); x++)
					dist[y, x] = long.MaxValue;

			var visited = new bool[map.GetLength(0), map.GetLength(1)];

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
					if (nodesConnected != null && nodesConnected(map[y1, x1], map[shortest.y, shortest.x]) == false)
						continue;

					var r = dist[shortest.y, shortest.x] + map[y1, x1];
					if (r < dist[y1, x1])
						dist[y1, x1] = r;
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

		/// <summary>
		/// TODO dijkstra make unified common method for later use
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="heightMap"></param>
		/// <param name="dist"></param>
		/// <param name="nodesConnected"></param>
		/// <returns></returns>
		public static (int x, int y)[] DijkstraPath((int x, int y) start, (int x, int y) end, int[,] heightMap, long[,] dist, 
			Func<int, int, bool> nodesConnected = null)
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

					// map points must be connected
					if (nodesConnected != null && nodesConnected(heightMap[pos.y, pos.x], heightMap[y1, x1]) == false)
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
	}
}
