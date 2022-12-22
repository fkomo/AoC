using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day16
{
	internal class Valve
	{
		public string Name { get; set; }
		public int FlowRate { get; set; }
		public Valve[] Tunnels { get; set; }
	}

	public class ProboscideaVolcanium : PuzzleBase
	{
		protected override (string, string) SolveProblem(string[] input)
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

			// part1
			long? answer1 = null;

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
