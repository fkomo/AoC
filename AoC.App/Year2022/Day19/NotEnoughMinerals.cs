using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day19
{
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

		internal struct State
		{
			public int TimeLeft;

			/// <summary>
			///	x: ore
			///	y: clay
			/// z: obsidian
			/// w: geode
			/// </summary>
			public v4i Collected;
			public v4i Robots;

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

			// part1 (5s / 14s)
			var qualities = new ConcurrentBag<long>();
			Parallel.ForEach(blueprints, bp =>
			{
				var maxGeodes = Step(bp, new State { Robots = Blueprint.OreCollectingRobot, TimeLeft = 24 }, new());
				qualities.Add(maxGeodes * bp.Id);

				Log.Line($"{bp.Id}: {maxGeodes * bp.Id}");
			});
			Log.Line($"part1: {string.Join(", ", qualities)}");

			long? answer1 = qualities.Sum();

			// part2
			long? answer2 = null;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private Blueprint[] ParseBlueprints(string[] input)
		{
			return input.Select(i =>
			{
				var s1 = i.Split(':');
				var s2 = s1[1].Split('.');

				return new Blueprint()
				{
					Id = int.Parse(s1[0]["Blueprint ".Length..]),
					OreRobotCost = new v4i(
						int.Parse(s2[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[4]),
						0,
						0,
						0),
					ClayRobotCost = new v4i(
						int.Parse(s2[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)[4]),
						0,
						0,
						0),
					ObsidianRobotCost = new v4i(
						int.Parse(s2[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[4]),
						int.Parse(s2[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)[7]),
						0,
						0),
					GeodeRobotCost = new v4i(
						int.Parse(s2[3].Split(' ', StringSplitOptions.RemoveEmptyEntries)[4]),
						0,
						int.Parse(s2[3].Split(' ', StringSplitOptions.RemoveEmptyEntries)[7]),
						0)
				};
			}).ToArray();
		}

		private static long Step(Blueprint bp, State state, Dictionary<string, long> cache)
		{
			if (state.TimeLeft == 0)
				return state.Collected.W;

			// memoization
			var cacheKey = state.ToString();
			if (cache.TryGetValue(cacheKey, out long maxGeode))
				return maxGeode;

			// build geode robot
			if (bp.CanBuild(Blueprint.Geode, state))
				maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Geode), cache));

			else
			{
				// build obsidian robot
				if (bp.CanBuild(Blueprint.Obsidian, state)
					&& state.Robots[Blueprint.Obsidian] < bp.GeodeRobotCost[Blueprint.Obsidian]
					)
					maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Obsidian), cache));

				// build clay robot
				if (bp.CanBuild(Blueprint.Clay, state)
					//&& state.TimeLeft > 3
					&& state.Robots[Blueprint.Clay] < bp.ObsidianRobotCost[Blueprint.Clay]
					)
					maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Clay), cache));

				// build ore robot
				if (bp.CanBuild(Blueprint.Ore, state) 
					&& state.Robots[Blueprint.Ore] < bp.RobotCosts.Max(c => c[Blueprint.Ore])
					)
					maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(bp, Blueprint.Ore), cache));

				// build nothing, just collect
				maxGeode = Math.Max(maxGeode, Step(bp, state.Advance(), cache));
			}

			cache.Add(cacheKey, maxGeode);

			return maxGeode;
		}
	}
}
