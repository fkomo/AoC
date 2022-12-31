using Ujeby.AoC.App.Year2022.Day09;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day15
{
	public static class Dijkstra
	{
		public static long[,] Create(int[,] weights, v2i start,
			Func<v2i, v2i, int[,], bool> connectionCheck = null)
		{
			var dist = new long[weights.GetLength(0), weights.GetLength(1)];
			for (var y = 0; y < weights.GetLength(0); y++)
				for (var x = 0; x < weights.GetLength(1); x++)
					dist[y, x] = long.MaxValue;

			var visited = new bool[weights.GetLength(0), weights.GetLength(1)];

			dist[start.Y, start.X] = 0;

			long shortestDist;
			var shortest = start;
			do
			{
				// visit
				foreach (var dir in Directions.NSWE)
				{
					var x1 = dir.Value[0] + shortest.X;
					var y1 = dir.Value[1] + shortest.Y;

					// visited or out of bounds
					if (x1 < 0 || y1 < 0 || x1 == weights.GetLength(1) || y1 == weights.GetLength(0) || visited[y1, x1])
						continue;

					if (connectionCheck != null && connectionCheck(new(x1, y1), shortest, weights) == false)
						continue;

					var r = dist[shortest.Y, shortest.X] + weights[y1, x1];
					if (r < dist[y1, x1])
						dist[y1, x1] = r;
				}
				visited[shortest.Y, shortest.X] = true;

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
							shortest = new(x, y);
						}
					}
			}
			while (shortestDist != long.MaxValue);

			return dist;
		}

		public static v2i[] Path(v2i start, v2i end, int[,] weights, long[,] dist, 
			Func<v2i, v2i, int[,], bool> connectionCheck = null)
		{
			var path = new List<v2i>();

			var pos = end;
			var bestDist = long.MaxValue;
			do
			{
				v2i? nextPos = null;
				foreach (var dir in Directions.NSWE)
				{
					var x1 = dir.Value[0] + pos.X;
					var y1 = dir.Value[1] + pos.Y;
					if (x1 < 0 || y1 < 0 || x1 == weights.GetLength(1) || y1 == weights.GetLength(0))
						continue;

					if (path.Any() && path.Last().X == x1 && path.Last().Y == y1)
						continue;

					if (connectionCheck != null && connectionCheck(pos, new(x1, y1), weights) == false)
						continue;

					if (dist[y1, x1] <= bestDist)
					{
						bestDist = dist[y1, x1];
						nextPos = new(x1, y1);
					}
				}

				pos = nextPos.Value;

				path.Add(pos);
			}
			while (pos.X != start.X || pos.Y != start.Y);

			path.Reverse();
			return path.ToArray();
		}
	}
}
