using Ujeby.AoC.App.Year2022.Day09;

namespace Ujeby.AoC.App.Year2021.Day15
{
	public static class Dijkstra
	{
		public static long[,] Create(int[,] weights, (int x, int y) start,
			Func<(int x, int y), (int x, int y), int[,], bool> connectionCheck = null)
		{
			var dist = new long[weights.GetLength(0), weights.GetLength(1)];
			for (var y = 0; y < weights.GetLength(0); y++)
				for (var x = 0; x < weights.GetLength(1); x++)
					dist[y, x] = long.MaxValue;

			var visited = new bool[weights.GetLength(0), weights.GetLength(1)];

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
					if (x1 < 0 || y1 < 0 || x1 == weights.GetLength(1) || y1 == weights.GetLength(0) || visited[y1, x1])
						continue;

					if (connectionCheck != null && connectionCheck((x1, y1), shortest, weights) == false)
						continue;

					var r = dist[shortest.y, shortest.x] + weights[y1, x1];
					if (r < dist[y1, x1])
						dist[y1, x1] = r;
				}
				visited[shortest.y, shortest.x] = true;

				// find next, shortest and not visited
				shortestDist = long.MaxValue;
				for (var y = 0; y < weights.GetLength(0); y++)
					for (var x = 0; x < weights.GetLength(1); x++)
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

			return dist;
		}

		public static (int x, int y)[] Path((int x, int y) start, (int x, int y) end, int[,] weights, long[,] dist, 
			Func<(int x, int y), (int x, int y), int[,], bool> connectionCheck = null)
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
					if (x1 < 0 || y1 < 0 || x1 == weights.GetLength(1) || y1 == weights.GetLength(0))
						continue;

					if (path.Any() && path.Last().x == x1 && path.Last().y == y1)
						continue;

					if (connectionCheck != null && connectionCheck(pos, (x1, y1), weights) == false)
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
