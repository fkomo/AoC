using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day16
{
	public class Valve
	{
		public string Name;
		public int FlowRate;
		public Valve[] Tunnels;
	}

	public class PipeState
	{
		public List<string> Opened;
		public string Current;
		public long PressureReleased;
	}

	public class ProboscideaVolcanium : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var valves = ParseValves(input);

			// part1
			//var pipes = new PipeState[]
			//{
			//	new PipeState
			//	{
			//		Current = "AA",
			//		Opened = new List<string>(),
			//		PressureReleased = 0,
			//	}
			//};

			//for (var i = 0; i < 30; i++)
			//{
			//	pipes = Step(valves, pipes);

			//	Debug.Line($"after minute {i + 1}");
			//	foreach (var p in pipes)
			//		Debug.Line($" + {p.Current} ({string.Join(',', p.Opened)}) = {p.PressureReleased}");
			//	Debug.Line();
			//}
			//long? answer1 = pipes.Max(p => p.PressureReleased);
			long? answer1 = null;

			// part2
			long? answer2 = null;

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

			foreach (var line in input)
			{
				var name = line.Substring("Valve ".Length, 2);

				var tStart = line.IndexOf("to valves ");
				if (tStart > 0)
					valves[name].Tunnels = line[(tStart + "to valves ".Length)..].Split(", ")
						.Select(v => valves[v])
						.ToArray();
				else
				{
					tStart = line.IndexOf("to valve ");
					valves[name].Tunnels = new Valve[] { valves[line[(tStart + "to valve ".Length)..]] };
				}
			}

			return valves;
		}

		public static PipeState[] Step(Dictionary<string, Valve> valves, PipeState[] pipes)
		{
			// add pressure
			foreach (var p in pipes)
				p.PressureReleased += p.Opened.Sum(v => valves[v].FlowRate);

			var newPipes = new List<PipeState>();
			foreach (var p in pipes)
			{
				// open valve if closed
				if (!p.Opened.Contains(p.Current) && valves[p.Current].FlowRate > 0)
					p.Opened.Add(p.Current);

				// move to other directions
				foreach (var t in valves[p.Current].Tunnels)
				{
					newPipes.Add(
						new PipeState
						{
							Current = t.Name,
							PressureReleased = p.PressureReleased,
							Opened = p.Opened.ToList(),
						});
				}

				// stay ?
			}

			return pipes.Concat(newPipes).ToArray();
		}
	}
}
