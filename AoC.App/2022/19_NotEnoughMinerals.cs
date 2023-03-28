using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2022_19
{
	[AoCPuzzle(Year = 2022, Day = 19, Answer1 = "1466", Answer2 = "8250", Skip = true)]
	public class NotEnoughMinerals : PuzzleBase
	{
		internal class Blueprint
		{
			static Blueprint()
			{
			}

			public const int Ore = 0;
			public const int Clay = 1;
			public const int Obsidian = 2;
			public const int Geode = 3;

			public int Id;

			public v4i OreRobotCost;
			public v4i ClayRobotCost;
			public v4i ObsidianRobotCost;
			public v4i GeodeRobotCost;

			public v4i[] RobotCosts => new[]
			{
				OreRobotCost,
				ClayRobotCost,
				ObsidianRobotCost,
				GeodeRobotCost
			};

			public v4i SumCost => OreRobotCost + ClayRobotCost + ObsidianRobotCost + GeodeRobotCost;

			public bool CanBuild(int robotIdx, State state)
			{
				var collected = state.Collected;
				return robotIdx switch
				{
					Ore => collected.X >= OreRobotCost.X,
					Clay => collected.X >= ClayRobotCost.X,
					Obsidian => collected.X >= ObsidianRobotCost.X && collected.Y >= ObsidianRobotCost.Y,
					Geode => collected.X >= GeodeRobotCost.X && collected.Z >= GeodeRobotCost.Z,
					_ => false,
				};
			}

			public static readonly v4i OreCollectingRobot = new(1, 0, 0, 0);
			public static readonly v4i ClayCollectingRobot = new(0, 1, 0, 0);
			public static readonly v4i ObsidianCollectingRobot = new(0, 0, 1, 0);
			public static readonly v4i GeideCrackingRobot = new(0, 0, 0, 1);
			public static readonly v4i[] Robots = new[]
			{
				OreCollectingRobot,
				ClayCollectingRobot,
				ObsidianCollectingRobot,
				GeideCrackingRobot
			};
		}

		internal record struct State(v4i Collected, v4i Robots, int TimeLeft)
		{
			public override string ToString() => $"{TimeLeft}{Collected}{Robots}";

			internal State Advance(
				Blueprint bp = null, int? robotToBuildIdx = null)
			{
				var result = this;

				if (robotToBuildIdx.HasValue)
				{
					result.Collected -= bp.RobotCosts[robotToBuildIdx.Value];
					result.Robots += Blueprint.Robots[robotToBuildIdx.Value];
				}

				result.Collected += Robots;
				result.TimeLeft = TimeLeft - 1;

				return result;
			}
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			var blueprints = ParseBlueprints(input);
			Debug.Line($"{blueprints.Length} blueprints");

			// TODO 2022/19 OPTIMIZE (105s)

			// part1 (5s)
			var results = new ConcurrentBag<long>();
			Parallel.ForEach(blueprints, bp =>
			{
				results.Add(bp.Id * Step(bp, new State { Robots = Blueprint.OreCollectingRobot, TimeLeft = 24 }, new()));
			});
			long? answer1 = results.Sum();

			// part2 (99s)
			results = new ConcurrentBag<long>();
			Parallel.ForEach(blueprints.Take(3), bp =>
			{
				results.Add(Step(bp, new State { Robots = Blueprint.OreCollectingRobot, TimeLeft = 32 }, new()));
			});
			long? answer2 = 1;
			foreach (var r in results)
				answer2 *= r;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private Blueprint[] ParseBlueprints(string[] input)
		{
			return input.Select(i =>
			{
				var n = i.ToNumArray();
				return new Blueprint()
				{
					Id = (int)n[0],
					OreRobotCost = new v4i(n[1], 0, 0, 0),
					ClayRobotCost = new v4i(n[2], 0, 0, 0),
					ObsidianRobotCost = new v4i(n[3], n[4], 0, 0),
					GeodeRobotCost = new v4i(n[5], 0, n[6], 0)
				};
			}).ToArray();
		}

		private static long Step(Blueprint bp, State state, Dictionary<string, long> cache)
		{
			if (state.TimeLeft == 0)
				return state.Collected[Blueprint.Geode];

			// memoization
			var cacheKey = state.ToString();
			if (cache.TryGetValue(cacheKey, out long maxGeode))
				return maxGeode;

			// build geode robot
			if (state.TimeLeft > 1 && bp.CanBuild(Blueprint.Geode, state))
				maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Geode), cache));

			else if (state.TimeLeft > 3)
			{
				// build obsidian robot
				if (bp.CanBuild(Blueprint.Obsidian, state)
					&& state.Robots[Blueprint.Obsidian] < bp.GeodeRobotCost[Blueprint.Obsidian]
					)
					maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Obsidian), cache));

				// build clay robot
				if (bp.CanBuild(Blueprint.Clay, state)
					&& state.TimeLeft > 7 // 7 is hardcoded for optimization
					&& state.Robots[Blueprint.Clay] < bp.ObsidianRobotCost[Blueprint.Clay]
					)
					maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Clay), cache));

				// build ore robot
				if (bp.CanBuild(Blueprint.Ore, state)
					&& state.Robots[Blueprint.Ore] < bp.RobotCosts.Max(c => c[Blueprint.Ore])
					)
					maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Ore), cache));
			}

			// build nothing, just collect
			maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(), cache));

			cache.Add(cacheKey, maxGeode);

			return maxGeode;
		}
	}
}
