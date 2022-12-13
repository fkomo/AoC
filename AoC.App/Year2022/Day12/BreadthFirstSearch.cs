using Ujeby.AoC.App.Year2022.Day09;

namespace Ujeby.AoC.App.Year2022.Day12
{
	public static class BreadthFirstSearch
	{
		public static (int x, int y)?[,] Create(int[,] map, (int x, int y) start,
			Func<(int x, int y), (int x, int y), int[,], bool> connectionCheck = null)
		{
			var visited = new bool[map.GetLength(0), map.GetLength(1)];
			var queue = new Queue<(int x, int y)>();

			var prev = new (int x, int y)?[map.GetLength(0), map.GetLength(1)];

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

					if (connectionCheck != null && connectionCheck((x1, y1), p, map) == false)
						continue;

					visited[y1, x1] = true;
					queue.Enqueue((x1, y1));
					prev[y1, x1] = p;
				}
			}
			while (queue.Count > 0);

			return prev;
		}

		public static (int x, int y)[] Path((int x, int y) start, (int x, int y) end, (int x, int y)?[,] prev)
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
