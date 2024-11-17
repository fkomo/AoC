using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_22;

[AoCPuzzle(Year = 2017, Day = 22, Answer1 = "5369", Answer2 = "2510774", Skip = true)]
public class SporificaVirus : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = 0;

		var dir = v2i.Down;
		var current = v2i.Zero;
		var infected = ParseInfected(input).ToList();

		var bursts = 10_000;
		for (; bursts-- > 0; current += dir)
		{
			dir = dir.Rotate90();
			if (!infected.Remove(current))
			{
				dir = dir.Inv();
				infected.Add(current);
				answer1++;
			}
		}

		// part2
		// TODO 2017/22 OPTIMIZE p2 (21s)
		var answer2 = 0;

		dir = v2i.Down;
		current = v2i.Zero;
		var nodes = ParseInfected(input).ToDictionary(x => x, x => NodeState.Infected);

		bursts = 10_000_000;
		for (; bursts-- > 0; current += dir)
		{
			if (!nodes.TryGetValue(current, out NodeState state))
			{
				state = NodeState.Clean;
				nodes.Add(current, state);
			}

			if (state == NodeState.Clean)
				dir = dir.Rotate90().Inv();

			else if (state == NodeState.Infected)
				dir = dir.Rotate90();

			else if (state == NodeState.Flagged)
				dir = dir.Inv();

			var newState = (NodeState)(((int)state + 1) % (int)NodeState.Count);
			if (newState == NodeState.Clean)
			{
				nodes.Remove(current);
				continue;
			}

			if (newState == NodeState.Infected)
				answer2++;

			nodes[current] = newState;
		}

		return (answer1.ToString(), answer2.ToString());
	}

	static v2i[] ParseInfected(string[] input)
	{
		var center = input.Length / 2;
		var infected = new List<v2i>();
		for (var y = 0; y < input.Length; y++)
			for (var x = 0; x < input.Length; x++)
				if (input[y][x] == '#')
					infected.Add(new v2i(x - center, y - center));

		return [.. infected];
	}

	enum NodeState : int
	{
		Clean = 0,
		Weakened,
		Infected,
		Flagged,

		Count
	}
}