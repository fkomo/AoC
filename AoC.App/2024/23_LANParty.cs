using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2024_23;

[AoCPuzzle(Year = 2024, Day = 23, Answer1 = "1184", Answer2 = null, Skip = false)]
public class LANParty : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var network = CreateNetwork(input);

		// part1
		var answer1 = FindSubnetsOf3(network).Count(x => x.StartsWith('t') || x.Contains(",t"));

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static string[] FindSubnetsOf3(Dictionary<string, List<string>> network)
	{
		var result = new HashSet<string>();

		foreach (var pc1 in network.Keys)
			foreach (var pc2 in network[pc1])
			{
				var pc3s = network[pc1].Intersect(network[pc2]).ToArray();
				if (pc3s.Length == 0)
					continue;

				foreach (var pc3 in pc3s)
					result.Add(string.Join(',', new string[] { pc1, pc2, pc3 }.OrderBy(x => x)));
			}

		return [.. result];
	}

	public static Dictionary<string, List<string>> CreateNetwork(string[] input)
	{
		var network = input.SelectMany(x => x.Split('-')).Distinct().ToDictionary(x => x, x => new List<string>());
		foreach (var connection in input.Select(x => x.Split('-')))
		{
			network[connection[0]].Add(connection[1]);
			network[connection[1]].Add(connection[0]);
		}

		return network;
	}
}