using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_16;

[AoCPuzzle(Year = 2024, Day = 16, Answer1 = null, Answer2 = null, Skip = false)]
public class ReindeerMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();

		// part1
		var answer1 = ShortestPath(map).Score();

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	public static v2i[] ShortestPath(char[][] map)
	{
		map.Find('S', out v2i start);
		map.Find('E', out v2i end);

		var nodes = new v2i[] { start, end }.Concat(map.EnumAll('.')).ToArray();
		var nodeIds = Enumerable.Range(0, nodes.Length).ToArray();

		long Weight(int n1, int n2)
		{
			return 1;
		}

		var neighbours = nodeIds.ToDictionary(
			x => x,
			x => v2i.UpDownLeftRight
				.Select(xx => Array.IndexOf(nodes, nodes[x] + xx))
				.Where(xx => xx != -1)
				.ToArray());

		int[] Neighbours(int node) => neighbours[node];

		return Ujeby.Alg.Graph.DijkstraShortestPath(0, 1, nodeIds, Neighbours, Weight)
			.Select(x => nodes[x])
			.ToArray();

		//return Ujeby.Alg.Graph.BreadthFirstSearch(0, 1, Neighbours)
		//	.Select(x => nodes[x])
		//	.ToArray();
	}
}

public static class Extensions
{
	public static long Score(this v2i[] path)
	{
		var score = path.Length;

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