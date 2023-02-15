using System.Collections.Concurrent;
using Ujeby.Alg;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_16
{
	/// <summary>
	/// inspired by https://observablehq.com/@a791ad12e8a3e3b4/advent-of-code-2022-day-16
	/// </summary>
	[AoCPuzzle(Year = 2022, Day = 16, Answer1 = "2029", Answer2 = "2723")]
	public class ProboscideaVolcanium : PuzzleBase
	{
		public class Valve
		{
			public string Name { get; set; }
			public int FlowRate { get; set; }
			public Valve[] Valves { get; set; }
			public int Idx { get; set; }

			public Valve(string name, int flowRate,
				Valve[] valves = null, int idx = -1)
			{
				Name = name;
				FlowRate = flowRate;
				Valves = valves;
				Idx = idx;
			}
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			var valves = ParseValves(input);
			var valveNames = valves.Keys.ToArray();

			// distance between any 2 valves
			var dist = new int[valveNames.Length, valveNames.Length];
			for (var y = 0; y < valveNames.Length; y++)
				for (var x = 0; x < valveNames.Length; x++)
					dist[y, x] = (y == x) ? 0 : 0xff;

			for (var valveFromIdx = 0; valveFromIdx < valveNames.Length; valveFromIdx++)
				foreach (var valveTo in valves[valveNames[valveFromIdx]].Valves)
				{
					dist[valveFromIdx, valveTo.Idx] = 1;
					dist[valveTo.Idx, valveFromIdx] = 1;
				}

			dist = Alg.FloydWarshall.ShortestPath(dist);

			Debug.Line($"all valves ({valves.Count}): {string.Join(", ", valves.Keys)}");
			foreach (var toRemove in valves.Where(v => v.Key != "AA" && v.Value.FlowRate == 0).Select(v => v.Key))
				valves.Remove(toRemove);
			Debug.Line($"nonZero valves ({valves.Count}): {string.Join(", ", valves.Keys)}");

			// part1
			var aaIdx = valves["AA"].Idx;
			long? answer1 = MoveToValve(valves, dist, Array.Empty<int>(), Array.Empty<int>(),
				valveIdx: aaIdx);

			// part2
			var noStartIdx = valves.Where(v => v.Key != "AA").Select(vk => vk.Value.Idx).ToArray();
			var halfLength = noStartIdx.Length / 2;

			var kComb = Combinatorics.KCombinations(noStartIdx, halfLength).ToArray();
			// only half of combinations is needed (the other will be mirrored)
			kComb = kComb.Take(kComb.Length / 2).ToArray();

			Debug.Line($"{kComb.Length} unique k-combinations of length={halfLength}");

			var pBag = new ConcurrentBag<int>();
			Parallel.ForEach(kComb, valves1 =>
			{
				var p1 = MoveToValve(valves, dist, Array.Empty<int>(), valves1.ToArray(),
					valveIdx: aaIdx,
					minutesLeft: 26);

				var p2 = MoveToValve(valves, dist, Array.Empty<int>(), noStartIdx.Except(valves1).ToArray(),
					valveIdx: aaIdx,
					minutesLeft: 26);

				pBag.Add(p1 + p2);
			});
			long? answer2 = pBag.Max();

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static Dictionary<string, Valve> ParseValves(string[] input)
		{
			var valves = input.Select(v =>
				new Valve(v.Substring("Valve ".Length, 2), int.Parse(v["Valve AA has flow rate=".Length..].Split(';')[0]))
				).ToDictionary(v => v.Name, v => v);

			for (var idx = 0; idx < input.Length; idx++)
			{
				var line = input[idx];
				var name = line.Substring("Valve ".Length, 2);
				valves[name].Idx = idx;

				var tStart = line.IndexOf("to valves ");
				if (tStart > 0)
					valves[name].Valves = line[(tStart + "to valves ".Length)..].Split(", ")
						.Select(v => valves[v])
						.ToArray();
				else
				{
					tStart = line.IndexOf("to valve ");
					valves[name].Valves = new Valve[] { valves[line[(tStart + "to valve ".Length)..]] };
				}
			}

			return valves;
		}

		private static int MoveToValve(Dictionary<string, Valve> valves, int[,] dist, int[] openedIdx, int[] ignoreIdx,
			int valveIdx = 0, int valveFlowRate = 0, int openedFlowRate = 0, int pressureReleased = 0, int minutesLeft = 30)
		{
			var bestPressure = int.MinValue;
			foreach (var nextValve in valves.Values)
			{
				var d = dist[valveIdx, nextValve.Idx] + 1;

				if (nextValve.Idx == valveIdx || openedIdx.Contains(nextValve.Idx) || d >= minutesLeft || ignoreIdx.Contains(nextValve.Idx))
					continue;

				var p = MoveToValve(valves, dist, openedIdx.Concat(new[] { valveIdx }).ToArray(), ignoreIdx,
					valveIdx: nextValve.Idx,
					valveFlowRate: nextValve.FlowRate,
					openedFlowRate: openedFlowRate + valveFlowRate,
					pressureReleased: pressureReleased + d * (valveFlowRate + openedFlowRate),
					minutesLeft: minutesLeft - d);

				if (p > bestPressure)
					bestPressure = p;
			}

			return (bestPressure == int.MinValue) ? pressureReleased + minutesLeft * (openedFlowRate + valveFlowRate) : bestPressure;
		}
	}
}