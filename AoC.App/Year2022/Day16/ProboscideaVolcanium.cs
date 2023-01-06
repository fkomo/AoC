using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day16
{
	public class Valve
	{
		public int Idx;
		public string Name;
		public int FlowRate;
		public Valve[] Valves;
	}

	public class ProboscideaVolcanium : PuzzleBase
	{
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

			foreach (var toRemove in valves.Where(v => v.Key != "AA" && v.Value.FlowRate == 0).Select(v => v.Key))
				valves.Remove(toRemove);

			// part1
			long? answer1 = MoveToValve(valves, dist, "AA", Array.Empty<string>(), out string[] path);
			Debug.Line(string.Join(", ", path));

			// part2
			long? answer2 = null;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static Dictionary<string, Valve> ParseValves(string[] input)
		{
			var valves = input.Select(v =>
				new Valve
				{
					Name = v.Substring("Valve ".Length, 2),
					FlowRate = int.Parse(v["Valve AA has flow rate=".Length..].Split(';')[0]),
				}).ToDictionary(v => v.Name, v => v);

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

		private static int MoveToValve(Dictionary<string, Valve> valves, int[,] dist,
			string valveName, string[] opened, out string[] path, 
			int openedFlowRate = 0, int pressureReleased = 0, int minutesLeft = 30)
		{
			var valve = valves[valveName];

			var bestPressure = int.MinValue;
			var bestPath = Array.Empty<string>();

			foreach (var nextValve in valves.Values)
			{
				var d = dist[valve.Idx, nextValve.Idx] + 1;

				if (nextValve.Idx == valve.Idx || opened.Contains(nextValve.Name) || (d + 1) >= minutesLeft)
					continue;

				var p = MoveToValve(valves, dist, nextValve.Name, 
					opened.Concat(new[] { valveName }).ToArray(),
					out path,
					openedFlowRate: openedFlowRate + valve.FlowRate,
					pressureReleased: pressureReleased + d * (valve.FlowRate + openedFlowRate),
					minutesLeft: minutesLeft - d);

				if (p > bestPressure)
				{
					bestPressure = p;
					bestPath = path.ToArray();
				}
			}

			// all valves are open
			if (bestPressure == int.MinValue)
			{
				path = opened.Concat(new[] { valveName }).ToArray();
				return pressureReleased + minutesLeft * (openedFlowRate + valve.FlowRate);
			}

			path = bestPath.ToArray();
			return bestPressure;
		}
	}
}