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

			Debug.Line($"dist after floyd-warshall:");
			for (var y = 0; y < valveNames.Length; y++)
			{
				var line = $"{dist[y, 0],3}";
				for (var x = 1; x < valveNames.Length; x++)
					line += $" {dist[y, x],3}";
				Debug.Line(line);
			}
			Debug.Line();

			// part1
			long? answer1 = MoveToValve(valves, dist, valves["AA"].Idx, Array.Empty<string>(), out _);

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

		private static int PressureReleased(Dictionary<string, Valve> valves, int minutes, params string[] opened)
			=> minutes * opened.Sum(v => valves[v].FlowRate);

		private static int MoveToValve(Dictionary<string, Valve> valves, int[,] dist, int valveIdx, string[] opened, out string[] path,
			int pressureReleased = 0, int minutesLeft = 30)
		{
			var valveNames = valves.Keys.ToArray();
			//Debug.Line($"{minutesLeft,2}min left - {valveNames[currentIdx]} - '{ string.Join(',', opened) }' => { pressureReleased }");

			Valve nextValve = GetNextValve(valves, dist, valveIdx, opened, minutesLeft);
			if (nextValve == null)
			{
				if (valveIdx != 0)
					opened = opened.Concat(new[] { valveNames[valveIdx] }).ToArray();
				pressureReleased += PressureReleased(valves, minutesLeft, opened);

				//Debug.Line($" 0min left - {valveNames[currentIdx]} - '{string.Join(',', opened)}' => {pressureReleased}");

				path = opened;
				Debug.Line($"{string.Join(',', path)} => {pressureReleased}");

				return pressureReleased;
			}

			var d = dist[valveIdx, nextValve.Idx] + 1;
			pressureReleased += PressureReleased(valves, d, opened) + PressureReleased(valves, d, valveNames[valveIdx]);
			if (valveIdx != 0)
				opened = opened.Concat(new[] { valveNames[valveIdx] }).ToArray();

			return MoveToValve(valves, dist, nextValve.Idx,
				opened, out path, pressureReleased, minutesLeft - d);
		}

		private static Valve GetNextValve(Dictionary<string, Valve> valves, int[,] dist, int currentIdx, string[] opened, int minutesLeft)
		{
			Valve bestRoute = null;
			var bestPressure = int.MinValue;
			foreach (var v in valves.Values)
			{
				if (v.Idx == currentIdx || v.FlowRate == 0 || opened.Contains(v.Name))
					continue;

				var pressure = PressureReleased(valves, minutesLeft - (dist[currentIdx, v.Idx] + 1), opened.Concat(new[] { v.Name }).ToArray());
				if (pressure > bestPressure)
				{
					bestRoute = v;
					bestPressure = pressure;
				}
			}

			return bestRoute;
		}
	}
}