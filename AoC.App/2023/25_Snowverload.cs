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
				x => x[5..].Split(' ').Select(n => (Idx: Array.IndexOf(nodes, n), Weight: 1L)).ToArray());
		Debug.Line($"{graph.Sum(x => x.Value.Length)} connections");

		var graph2way = Fix1WayGraphAs2Way(graph);
		Debug.Line($"{graph2way.Sum(x => x.Value.Length)} 2way connections");

		// part1
		// sample paths between random nodes and find most used
		var segmentUsage = new Dictionary<(int From, int To), long>();
		var rnd = new Random();
		for (var i = 0; i < 100; i++)
		{
			var n1 = rnd.Next(nodes.Length);
			var n2 = rnd.Next(nodes.Length);

			var sp = Dijkstra.ShortestPath(graph2way, graph2way.Keys.ToArray(), n1, n2);
			if (sp.Length > 1)
			{
				for (var p = 0; p < sp.Length - 1; p++)
				{
					var key = (System.Math.Min(sp[p], sp[p + 1]), System.Math.Max(sp[p], sp[p + 1]));
					if (!segmentUsage.ContainsKey(key))
						segmentUsage.Add(key, 1);
					else
						segmentUsage[key]++;
				}
			}
		}

		// remove 3 most used connections
		var top3 = segmentUsage
			.OrderByDescending(x => x.Value)
			.Take(3);
		foreach (var s in top3)
		{
			graph2way[s.Key.From] = graph2way[s.Key.From].Where(x => x.Idx != s.Key.To).ToArray();
			graph2way[s.Key.To] = graph2way[s.Key.To].Where(x => x.Idx != s.Key.From).ToArray();
		}

		var count = CountNodes(graph2way, graph2way.Keys.First());
		var answer1 = (graph2way.Keys.Count - count) * count;

		// part2
		string answer2 = "*";

		return (answer1.ToString(), answer2.ToString());
	}

	static int CountNodes(Dictionary<int, (int Idx, long Weight)[]> graph, int source)
	{
		var counted = new HashSet<int>();

		var nodeQueue = new Queue<int>();
		nodeQueue.Enqueue(source);
		while (nodeQueue.Any())
		{
			var node = nodeQueue.Dequeue();
			if (!counted.Add(node))
				continue;

			foreach (var (Idx, Weight) in graph[node].Where(x => !counted.Contains(x.Idx)))
				nodeQueue.Enqueue(Idx);
		}

		return counted.Count;
	}

	static Dictionary<int, (int Idx, long Weight)[]> Fix1WayGraphAs2Way(Dictionary<int, (int Idx, long Weight)[]> graph)
	{
		var fix = new Dictionary<int, HashSet<(int, long)>>();
		foreach (var from in graph)
		{
			if (!fix.ContainsKey(from.Key))
				fix.Add(from.Key, new());

			foreach (var to in from.Value)
			{
				fix[from.Key].Add((to.Idx, to.Weight));

				if (!fix.ContainsKey(to.Idx))
					fix.Add(to.Idx, new());

				fix[to.Idx].Add((from.Key, to.Weight));
			}
		}

		return fix.ToDictionary(x => x.Key, x => x.Value.ToArray());
	}
}