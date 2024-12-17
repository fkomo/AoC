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
		var shortestPath = ShortestPath(map);

		var answer1 = 0;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	public static v2i[] ShortestPath(char[][] map)
	{
		map.Find('S', out v2i start);
		map.Find('E', out v2i end);

		var nodes = new v2i[] { start, end }.Concat(map.EnumAll('.')).ToArray();

		//long Weight(int n1, int n2)
		//{
		//	return 1;
		//}

		//int[] Neighbours(int node) => 
		//	v2i.UpDownLeftRight
		//		.Select(x => Array.IndexOf(nodes, nodes[node] + x))
		//		.Where(x => x != -1)
		//		.ToArray();

		//return Ujeby.Alg.Graph.DijkstraShortestPath(0, 1, Enumerable.Range(0, nodes.Length).ToArray(), Neighbours, Weight)
		//	.Select(x => nodes[x])
		//	.ToArray();

		bool Connection(v2i n1, v2i n2)
		{
			return map.Get(n1) != '#' &&  map.Get(n2) != '#';
		}

		return Ujeby.Alg.Graph.BreadthFirstSearch(start, end, map.ToAAB2i().Size, Connection);
	}
}