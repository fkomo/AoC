using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day15
{
	public static class AStar
	{
		public static long[,] Create(int[,] weights, v2i start, v2i end, Func<v2i, v2i, long> hFunc, out int[,] prev)
		{
			var size = new v2i(weights.GetLength(1), weights.GetLength(0));

			prev = new int[size.Y, size.X]; // 0, 1, 2, 3 (RightDownLeftUp)

			var heur = new long[size.Y, size.X];
			var dist = new long[size.Y, size.X];
			var visited = new bool[size.Y, size.X];

			var unvisited = new List<v2i>();
			for (int i = 0, y = 0; y < size.Y; y++)
				for (var x = 0; x < size.X; x++, i++)
				{
					dist[y, x] = long.MaxValue;
					heur[y, x] = long.MaxValue;
					prev[y, x] = -1;
					unvisited.Add(new(x, y));
				}

			dist[start.Y, start.X] = 0;
			heur[start.Y, start.X] = dist[start.Y, start.X] + hFunc(start, start);

			while (unvisited.Any())
			{
				var uMinIdx = -1;
				var uMinScore = long.MaxValue;
				for (var uIdx = 0; uIdx < unvisited.Count; uIdx++)
				{
					var qq = unvisited[uIdx];
					if (heur[qq.Y, qq.X] < uMinScore)
					{
						uMinIdx = uIdx;
						uMinScore = heur[qq.Y, qq.X];
					}
				}

				var u = unvisited[uMinIdx];

				if (u == end)
					return dist;

				unvisited.RemoveAt(uMinIdx);
				visited[u.Y, u.X] = true;

				var rightDownLeftUpLength = v2i.RightDownLeftUp.Length;
				for (var vDir = 0; vDir < rightDownLeftUpLength; vDir++)
				{
					var v = u + v2i.RightDownLeftUp[vDir];

					// visited or out of bounds
					if (v.X < 0 || v.Y < 0 || v.X == size.X || v.Y == size.Y || visited[v.Y, v.X])
						continue;

					var d = dist[u.Y, u.X] + weights[v.Y, v.X];
					if (d < dist[v.Y, v.X])
					{
						dist[v.Y, v.X] = d;
						prev[v.Y, v.X] = (vDir + 2) % rightDownLeftUpLength;
						heur[v.Y, v.X] = dist[v.Y, v.X] + hFunc(u, v);
					}
				}
			}

			return null;
		}

		public static v2i[] Path(v2i start, v2i end, int[,] prev)
		{
			List<v2i> path = new()
			{
				end
			};

			while (path.Last() != start)
				path.Add(path.Last() + v2i.RightDownLeftUp[prev[path.Last().Y, path.Last().X]]);

			path.Reverse();
			return path.ToArray();
		}
	}
}
