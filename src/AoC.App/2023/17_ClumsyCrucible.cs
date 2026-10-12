using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_17;

[AoCPuzzle(Year = 2023, Day = 17, Answer1 = "1155", Answer2 = "1283", Skip = true)]
public class ClumsyCrucible : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var grid = input
			.Select(y => y.Select(x => x - '0').ToArray())
			.ToArray();

		// part1
		var answer1 = MinHeatLoss(grid, new v2i(0), new v2i(grid.Length - 1));
		// TODO 2023/17 OPTIMIZE p1 (12s)

		// part2
		var answer2 = MinHeatLossPart2(grid, new v2i(0), new v2i(grid.Length - 1));
		// TODO 2023/17 OPTIMIZE p2 (2min)

		return (answer1.ToString(), answer2.ToString());
	}

	record class State(v2i Pos, v2i Dir, int Dist);

	static long MinHeatLoss(int[][] grid, v2i source, v2i target)
	{
		var targetCost = long.MaxValue;
		var stateQueue = new List<(State State, long Cost)>();
		var visitedStates = new HashSet<State>();

		bool AddState(long cost, v2i p, v2i dir, int distance, out long targetCost)
		{
			p += dir;

			targetCost = cost;
			if (p.X < 0 || p.Y < 0 || p.X >= grid.Length || p.Y >= grid.Length)
				return false;

			cost += grid[p.Y][p.X];
			if (p == target)
			{
				targetCost = cost;
				return true;
			}

			var state = new State(p, dir, distance);
			if (visitedStates.Add(state))
				stateQueue.Add((state, cost));

			return false;
		}

		AddState(0, source, v2i.Up, 1, out _);
		AddState(0, source, v2i.Right, 1, out _);
		while (stateQueue.Any())
		{
			var minCost = stateQueue.Min(x => x.Cost);
			var nextStates = stateQueue.Where(x => x.Cost == minCost).ToArray();
			stateQueue.RemoveAll(x => nextStates.Contains(x));

			foreach (var state in nextStates)
			{
				if (AddState(minCost, state.State.Pos, new v2i(state.State.Dir.Y, -state.State.Dir.X), 1, out targetCost) ||
					AddState(minCost, state.State.Pos, new v2i(-state.State.Dir.Y, state.State.Dir.X), 1, out targetCost))
					return targetCost;

				if (state.State.Dist < 3)
					if (AddState(minCost, state.State.Pos, state.State.Dir, state.State.Dist + 1, out targetCost))
						return targetCost;
			}
		}

		return targetCost;
	}

	static long MinHeatLossPart2(int[][] grid, v2i source, v2i target)
	{
		var targetCost = long.MaxValue;
		var stateQueue = new List<(State State, long Cost)>();
		var visitedStates = new HashSet<State>();

		bool AddState(long cost, v2i p, v2i dir, int distance, out long targetCost)
		{
			p += dir;

			targetCost = cost;
			if (p.X < 0 || p.Y < 0 || p.X >= grid.Length || p.Y >= grid.Length)
				return false;

			cost += grid[p.Y][p.X];
			if (p == target && distance >= 4 && distance <= 10)
			{
				targetCost = cost;
				return true;
			}

			var state = new State(p, dir, distance);
			if (visitedStates.Add(state))
				stateQueue.Add((state, cost));

			return false;
		}

		AddState(0, source, v2i.Up, 1, out _);
		AddState(0, source, v2i.Right, 1, out _);
		while (stateQueue.Any())
		{
			var minCost = stateQueue.Min(x => x.Cost);
			var nextStates = stateQueue.Where(x => x.Cost == minCost).ToArray();
			stateQueue.RemoveAll(x => nextStates.Contains(x));

			foreach (var state in nextStates)
			{
				if (state.State.Dist >= 4)
					if (AddState(minCost, state.State.Pos, new v2i(state.State.Dir.Y, -state.State.Dir.X), 1, out targetCost) ||
						AddState(minCost, state.State.Pos, new v2i(-state.State.Dir.Y, state.State.Dir.X), 1, out targetCost))
						return targetCost;

				if (state.State.Dist < 10)
					if (AddState(minCost, state.State.Pos, state.State.Dir, state.State.Dist + 1, out targetCost))
						return targetCost;
			}
		}

		return targetCost;
	}
}