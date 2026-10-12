using Ujeby.Vectors;

namespace Ujeby.Alg
{
	public static class Graph
	{
		/// <summary>
		/// when graph contains only from -> to connections, this will fix it, so also reverse (to -> from) connections are present
		/// </summary>
		/// <param name="graph">1way graph</param>
		/// <returns>2way graph</returns>
		public static Dictionary<int, (int Dest, long Weight)[]> Fix1WayAs2Way(this Dictionary<int, (int Dest, long Weight)[]> graph)
		{
			var fix = new Dictionary<int, HashSet<(int, long)>>();
			foreach (var from in graph)
			{
				if (!fix.ContainsKey(from.Key))
					fix.Add(from.Key, []);

				foreach (var (Dest, Weight) in from.Value)
				{
					fix[from.Key].Add((Dest, Weight));

					if (!fix.ContainsKey(Dest))
						fix.Add(Dest, []);

					fix[Dest].Add((from.Key, Weight));
				}
			}

			return fix.ToDictionary(x => x.Key, x => x.Value.ToArray());
		}

		public static long CountReach(this Dictionary<int, (int Dest, long Weight)[]> graph, int source)
		{
			var counted = new HashSet<int>();

			var nodeQueue = new Queue<int>();
			nodeQueue.Enqueue(source);
			while (nodeQueue.Count != 0)
			{
				var node = nodeQueue.Dequeue();
				if (!counted.Add(node))
					continue;

				foreach (var (Dest, Weight) in graph[node].Where(x => !counted.Contains(x.Dest)))
					nodeQueue.Enqueue(Dest);
			}

			return counted.Count;
		}

		public static int[] DijkstraShortestPath(this Dictionary<int, (int Dest, long Weight)[]> graph, int source, int target, 
			int[] nodes = null)
			=> DijkstraShortestPath(
				source, 
				target, 
				nodes ?? [.. graph.Keys], 
				(node) => graph[node].Select(x => x.Dest).ToArray(), 
				(n1, n2) => graph[n1].Single(x => x.Dest == n2).Weight);

		public static int[] DijkstraShortestPath(int start, int end, int[] nodes, Func<int, int[]> neighbours, Func<int, int, long> weight)
		{
			var q = new List<int>();
			var visited = new Dictionary<int, bool>();
			var prev = new Dictionary<int, int>();
			var dist = new Dictionary<int, long>();

			foreach (var v in nodes)
			{
				visited.Add(v, false);
				dist.Add(v, long.MaxValue);
				prev.Add(v, -1);
				q.Add(v);
			}
			dist[start] = 0;

			// shortest path from source to target
			while (q.Count != 0)
			{
				var u = q.OrderBy(x => dist[x]).First();
				q.Remove(u);

				visited[u] = true;

				if (u == end)
					break;

				foreach (var n in neighbours(u).Where(x => !visited[x]))
				{
					var d = dist[u] + weight(u, n);
					if (d < dist[n])
					{
						dist[n] = d;
						prev[n] = u;
					}
				}
			}

			return Path(prev, start, end);
		}

		public static int[] BreadthFirstSearch(int start, int end, Func<int, int[]> neighbours)
		{
			var queue = new Queue<int>();
			var prev = new Dictionary<int, int>();
			var visited = new HashSet<int>();

			queue.Enqueue(start);
			visited.Add(start);

			while (queue.Count != 0)
			{
				var q = queue.Dequeue();
				foreach (var n in neighbours(q))
				{
					if (!visited.Add(n))
						continue;

					queue.Enqueue(n);
					prev.Add(n, q);
				}
			}

			return Path(prev, start, end);
		}

		public static int[] DepthFirstSearch(int start, int end, Func<int, int[]> neighbours)
		{
			var queue = new Stack<int>();
			var prev = new Dictionary<int, int>();
			var visited = new HashSet<int>();

			queue.Push(start);
			visited.Add(start);

			while (queue.Count != 0)
			{
				var q = queue.Pop();
				foreach (var n in neighbours(q))
				{
					if (!visited.Add(n))
						continue;

					queue.Push(n);
					prev.Add(n, q);
				}
			}

			return Path(prev, start, end);
		}

		static int[] Path(Dictionary<int, int> prev, int start, int end)
		{
			var path = new List<int>() { end };
			while (path[^1] != start)
			{
				if (!prev.TryGetValue(path[^1], out int p))
					return [];

				path.Add(p);
			}

			path.Reverse();

			return [.. path];
		}

		/// <summary>
		/// returns distance to all reachable tiles from start with distFunc (>0)
		/// </summary>
		/// <param name="start"></param>
		/// <param name="directions"></param>
		/// <returns></returns>
		public static Dictionary<v2i, int> SeedFillDistance(v2i start, v2i[] directions, Func<v2i, v2i, int> distFunc)
		{
			var fillDistance = new Dictionary<v2i, int>();

			var queue = new Queue<(v2i p, int distance)>();
			queue.Enqueue((start, 0));

			while (queue.Count > 0)
			{
				var (p, distance) = queue.Dequeue();

				if (!fillDistance.TryAdd(p, distance))
					continue;

				for (var i = 0; i < directions.Length; i++)
				{
					var df = distFunc(p, p + directions[i]);
					if (df <= 0)
						continue;

					queue.Enqueue((p + directions[i], distance + df));
				}
			}

			return fillDistance;
		}
	}
}
