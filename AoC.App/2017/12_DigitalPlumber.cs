using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2017_12;

[AoCPuzzle(Year = 2017, Day = 12, Answer1 = "113", Answer2 = "202", Skip = false)]
public class DigitalPlumber : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var pipes = input.Select(x => x.ToNumArray().Select(x => (int)x).ToArray()).ToDictionary(x => x[0], x => x[1..]);

		// part1
		var answer1 = pipes.Keys.Count(x => TravelTo(0, pipes, x));

		// part2
		var groups = new HashSet<byte[]>(new ByteArrayComparer());
		var nodes = pipes.ToDictionary(x => x.Key, x => false);

		foreach (var key in nodes.Keys)
		{
			if (nodes[key])
				continue;

			var group = new List<int>()
				.GroupAround(key, pipes, nodes)
				.ToArray()
				.ToByteArray();

			groups.Add(group);
		}
		var answer2 = groups.Count;

		return (answer1.ToString(), answer2.ToString());
	}

	static bool TravelTo(int to, Dictionary<int, int[]> pipes, params int[] path) =>
		pipes[path[^1]].Any(x => x == to || !path.Contains(x) && TravelTo(to, pipes, [.. path, .. new int[] { x }]));
}

static class Extensions
{
	public static List<int> GroupAround(this List<int> group, int center, Dictionary<int, int[]> pipes, Dictionary<int, bool> nodes)
	{
		group.Add(center);
		nodes[center] = true;

		foreach (var neighbour in pipes[center])
		{
			if (nodes[neighbour])
				continue;

			group.GroupAround(neighbour, pipes, nodes);
		}

		return group;
	}

	public static byte[] ToByteArray(this int[] array)
	{
		var result = new byte[array.Length * sizeof(int)];
		Buffer.BlockCopy(array, 0, result, 0, result.Length);

		return result;
	}
}