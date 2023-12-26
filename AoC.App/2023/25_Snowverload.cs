using System.Collections.Concurrent;
using Ujeby.Alg;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_25;

[AoCPuzzle(Year = 2023, Day = 25, Answer1 = "548960", Answer2 = "*", Skip = false)]
public class Snowverload : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var nodes = input
			.SelectMany(x => x.Replace(":", "").Split(' '))
			.Distinct()
			.ToArray();
		Debug.Line($"{nodes.Length} nodes");

		var graph = input
			.ToDictionary(
				x => Array.IndexOf(nodes, x[..3]), 
				x => x[5..].Split(' ').Select(n => (Dest: Array.IndexOf(nodes, n), Weight: 1L)).ToArray())
			.Fix1WayAs2Way();

		Debug.Line($"{graph.Sum(x => x.Value.Length)} connections");

		// part1
		// sample paths between random nodes and find most used
		var rnd = new Random(0);
		var segmentsBag = new ConcurrentBag<(int From, int To)>();
		Parallel.For(0, 100, (i) =>
		{
			var n1 = rnd.Next(nodes.Length);
			var n2 = rnd.Next(nodes.Length);
			var sp = Dijkstra.ShortestPath(graph, graph.Keys.ToArray(), n1, n2);
			for (var p = 0; p < sp.Length - 1; p++)
				segmentsBag.Add((System.Math.Min(sp[p], sp[p + 1]), System.Math.Max(sp[p], sp[p + 1])));
		});

		// remove 3 most used connections from graph
		var top3 = segmentsBag
			.GroupBy(x => x)
			.OrderByDescending(x => x.Count())
			.Take(3);
		foreach (var s in top3)
		{
			graph[s.Key.From] = graph[s.Key.From].Where(x => x.Dest != s.Key.To).ToArray();
			graph[s.Key.To] = graph[s.Key.To].Where(x => x.Dest != s.Key.From).ToArray();
		}

		var count = graph.CountReach(graph.Keys.First());
		var answer1 = (graph.Keys.Count - count) * count;

		// part2
		string answer2 = "*";

		return (answer1.ToString(), answer2.ToString());
	}
}