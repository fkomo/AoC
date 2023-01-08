using FFMpegCore.Enums;
using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day19
{
	public class NotEnoughMinerals : PuzzleBase
	{
		internal class Blueprint
		{
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

			public bool CanBuild(int robotIdx, v4i collected)
			{
				switch (robotIdx)
				{
					case 0: return collected.X >= OreRobotCost.X; 
					case 1: return collected.X >= ClayRobotCost.X;
					case 2: return collected.X >= ObsidianRobotCost.X && collected.Y >= ObsidianRobotCost.Y;
					case 3: return collected.X >= ObsidianRobotCost.X && collected.Z >= GeodeRobotCost.Z;
					default: return false;
				}

				//var cost = RobotCosts[robotIdx];
				//for (var c = 0; c < 4; c++)
				//{
				//	if (cost[c] == 0)
				//		continue;

				//	if (cost[c] > collected[c])
				//		return false;
				//}

				//return true;
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
			/// <summary>
			///	x: ore
			///	y: clay
			/// z: obsidian
			/// w: geode
			/// </summary>
			public v4i Collected;
			public v4i Robots;

			public override string ToString() => $"{Collected},{Robots}";

			internal State BuildAndCollect(Blueprint bp, int robotToBuildIdx)
			{
				var result = this;

				result.Collected -= bp.RobotCosts[robotToBuildIdx];
				result.Robots += Blueprint.Robots[robotToBuildIdx];
				result.Collected += Robots;

				return result;
			}

			internal State Collect()
			{
				var result = this;

				result.Collected += Robots;

				return result;
			}
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			var blueprints = ParseBlueprints(input);
			Debug.Line($"{blueprints.Length} blueprints");

			// part1 (14s)
			var qualities = new ConcurrentBag<long>();
			Parallel.ForEach(blueprints, bp =>
			{
				var cache = new Dictionary<string, State>();

				var finalState = Step(bp, new State { Robots = new v4i(1, 0, 0, 0) }, cache);
				qualities.Add(finalState.Collected.W * bp.Id);
				Log.Line($"{bp.Id}: {finalState.Collected.W * bp.Id}");
			});
			Log.Line($"p1: {string.Join(", ", qualities)}");

			long? answer1 = qualities.Sum(); // 33
			// 1403 too low

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

		private static State Step(Blueprint bp, State state, Dictionary<string, State> cache,
			int minutesLeft = 24)
		{
			if (minutesLeft == 0)
				return state;

			// memoization
			var cacheKey = $"{minutesLeft}{state}";
			if (cache.ContainsKey(cacheKey))
				return cache[cacheKey];

			minutesLeft--;

			var bs = state;
			State s;

			var canBuild3 = bp.CanBuild(3, state.Collected);
			if (!canBuild3 && minutesLeft > 1)
			{
				// build robot 2
				if (bp.CanBuild(2, state.Collected) && 
					state.Robots[2] < bp.GeodeRobotCost[2])
				{
					s = Step(bp, state.BuildAndCollect(bp, 2), cache,
						minutesLeft: minutesLeft);
					if (s.Collected.W > bs.Collected.W)
						bs = s;
				}

				// build robot 1
				if (minutesLeft > 2 && 
					bp.CanBuild(1, state.Collected) && 
					state.Robots[1] < bp.ObsidianRobotCost[1])
				{
					s = Step(bp, state.BuildAndCollect(bp, 1), cache,
						minutesLeft: minutesLeft);
					if (s.Collected.W > bs.Collected.W)
						bs = s;
				}

				// build robot 0
				if (bp.CanBuild(0, state.Collected) && 
					state.Robots[0] < bp.RobotCosts.Max(c => c[0]))
				{
					s = Step(bp, state.BuildAndCollect(bp, 0), cache,
						minutesLeft: minutesLeft);
					if (s.Collected.W > bs.Collected.W)
						bs = s;
				}
			}
			// build robot 3
			else if (minutesLeft > 0 && canBuild3)
			{
				s = Step(bp, state.BuildAndCollect(bp, 3), cache,
					minutesLeft: minutesLeft);
				if (s.Collected.W > bs.Collected.W)
					bs = s;
			}

			// build nothing, just collect
			s = Step(bp, state.Collect(), cache,
				minutesLeft: minutesLeft);
			if (s.Collected.W > bs.Collected.W)
				bs = s;

			cache.Add(cacheKey, bs);

			return bs;
		}
	}
}
