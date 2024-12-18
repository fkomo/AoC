using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_16;

[AoCPuzzle(Year = 2024, Day = 16, Answer1 = "93436", Answer2 = null, Skip = false)]
public class ReindeerMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		map.Find('S', out v2i start);
		map.Find('E', out v2i end);

		// part1
		// TODO OPTIMIZE 2024/16 p1 (2s)
		var nodes = new v2i[] { start, end }.Concat(map.EnumAll('.')).ToArray();
		var answer1 = CustomBreadthFirstSearchScore(nodes, start, end);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static int CustomBreadthFirstSearchScore(v2i[] nodes, v2i start, v2i end)
	{
		(v2i Dir, int Score)[] Neighbours(v2i node, v2i dir)
		{
			var result = new List<(v2i Pos, int Score)>();

			if (nodes.Contains(node + dir))
				result.Add((dir, 1));

			if (nodes.Contains(node + dir.RotateCCW()))
				result.Add((dir.RotateCCW(), 1001));

			if (nodes.Contains(node + dir.RotateCW()))
				result.Add((dir.RotateCW(), 1001));

			return [.. result];
		}

		var queue = new Queue<(v2i Pos, v2i Dir, int Score)>();
		var visited = new Dictionary<v2i, int>();

		queue.Enqueue((start, v2i.Right, 0));
		visited.Add(start, 0);

		while (queue.Count != 0)
		{
			var q = queue.Dequeue();
			foreach (var n in Neighbours(q.Pos, q.Dir))
			{
				var qnPos = q.Pos + n.Dir;
				var qnScore = q.Score + n.Score;

				if (visited.TryGetValue(qnPos, out var visitedScore) && visitedScore <= qnScore)
					continue;

				visited[qnPos] = qnScore;

				queue.Enqueue((qnPos, n.Dir, qnScore));
			}
		}

		return visited[end];
	}
}