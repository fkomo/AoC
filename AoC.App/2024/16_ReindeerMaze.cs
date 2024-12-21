using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_16;

[AoCPuzzle(Year = 2024, Day = 16, Answer1 = "93436", Answer2 = "486", Skip = true)]
public class ReindeerMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		map.Find('S', out v2i start);
		map.Find('E', out v2i end);

		// part1
		// TODO OPTIMIZE 2024/16 p1 (2s)
		var visited = CustomBreadthFirstSearch(new v2i[] { start, end }.Concat(map.EnumAll('.')).ToArray(), start);
		var answer1 = visited[end];

		// part2
		// TODO OPTIMIZE 2024/16 p2 (20s)
		var answer2 = PickBestPlaces(visited, start, end).Length;

		return (answer1.ToString(), answer2.ToString());
	}

	public static Dictionary<v2i, int> CustomBreadthFirstSearch(v2i[] nodes, v2i start)
	{
		(v2i Dir, int Score)[] NeighboursWithScore(v2i node, v2i dir) =>
			new (v2i Dir, int Score)[] { (dir, 1), (dir.RotateCCW(), 1001), (dir.RotateCW(), 1001) }
				.Where(x => nodes.Contains(node + x.Dir))
				.ToArray();

		var queue = new Queue<(v2i Pos, v2i Dir, int Score)>();
		var visited = new Dictionary<v2i, int>();

		queue.Enqueue((start, v2i.Right, 0));
		visited.Add(start, 0);

		while (queue.Count != 0)
		{
			var q = queue.Dequeue();
			foreach (var n in NeighboursWithScore(q.Pos, q.Dir))
			{
				var qnPos = q.Pos + n.Dir;
				var qnScore = q.Score + n.Score;

				if (visited.TryGetValue(qnPos, out var visitedScore) && visitedScore <= qnScore)
					continue;

				visited[qnPos] = qnScore;

				queue.Enqueue((qnPos, n.Dir, qnScore));
			}
		}

		return visited;
	}

	public static v2i[] PickBestPlaces(Dictionary<v2i, int> visited, v2i start, v2i end)
	{
		var places = visited.Keys.ToArray();

		v2i[] Neighbours(v2i node) => v2i.UpDownLeftRight.Select(x => x + node).Where(x => places.Contains(x)).ToArray();

		// stage1 - backtrack visited nodes from end to start a pick possible-best-places (some are wrong)
		var queue = new Queue<(v2i Pos, int Score)>();
		var possibleBestPlaces = new HashSet<v2i>();

		queue.Enqueue((end, visited[end]));
		possibleBestPlaces.Add(end);

		while (queue.Count != 0)
		{
			var q = queue.Dequeue();
			foreach (var n in Neighbours(q.Pos))
			{
				if (visited[n] >= visited[end])
					continue;

				if (visited[n] == q.Score - 1 || visited[n] == q.Score - 1001 || visited[n] == q.Score + 999)
				{
					if (!possibleBestPlaces.Add(n))
						continue;

					if (n != start)
						queue.Enqueue((n, visited[n]));
				}
			}
		}

		var bestPathScore = visited[end];

		// stage2 - find all best paths in possible-best-places and therefore filter the best places!
		var bestPlaces = new HashSet<v2i>();
		places = [.. possibleBestPlaces];

		var crossroads = new Queue<HashSet<v2i>>();
		crossroads.Enqueue([start]);

		while (crossroads.Count > 0)
		{
			var path = crossroads.Dequeue();

			var p = path.Last();
			foreach (var n in Neighbours(p))
			{
				if (path.Contains(n))
					continue;

				var path2 = path.ToHashSet();
				path2.Add(n);

				if (n != end)
					crossroads.Enqueue(path2);

				else if (path2.ToArray().Score() == bestPathScore)
					foreach (var bp in path2)
						bestPlaces.Add(bp);
			}
		}

		return [.. bestPlaces];
	}
}

public static class Extensions
{
	public static long Score(this v2i[] path)
	{
		var score = path.Length - 1;

		var dir = v2i.Right;
		for (var i = 1; i < path.Length; i++)
		{
			if (path[i] - path[i - 1] != dir)
				score += 1000;

			dir = path[i] - path[i - 1];
		}

		return score;
	}
}