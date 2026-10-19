using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2024_23;

[AoCPuzzle(Year = 2024, Day = 23, Answer1 = "1184", Answer2 = "hf,hz,lb,lm,ls,my,ps,qu,ra,uc,vi,xz,yv", Skip = true)]
public class LANParty : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var network = CreateNetwork(input);

		// part1
		var subnetsOf3 = FindSubnetsOf3(network);
		var answer1 = subnetsOf3.Count(x => x.StartsWith('t') || x.Contains(",t"));

		// part2
		// TODO 2024/23 OPTIMIZE p2 (4s)
		var answer2 = FindBiggestSubnet(network);

		return (answer1.ToString(), answer2.ToString());
	}

	static string FindBiggestSubnet(Dictionary<string, List<string>> network)
	{
		var result = string.Empty;

		foreach (var pc in network.Keys)
		{
			var subnetSize = network[pc].Count;

			while (subnetSize >= 3)
			{
				foreach (var subnet in Alg.Combinatorics.Combinations(network[pc], subnetSize)
					.Where(x => ValidateSubnet(network, x.ToArray())))
				{
					var password = string.Join(",", subnet.Concat([pc]).OrderBy(x => x));
					if (subnetSize + 1 == network[pc].Count) // subnet can't be bigger than this
						return password;

					if (password.Length > result.Length)
						result = password;
				}

				subnetSize--;
			}
		}

		return result;
	}

	static bool ValidateSubnet(Dictionary<string, List<string>> network, string[] subnet)
	{
		foreach (var pc2 in subnet)
		{
			var subnetExceptPc2 = subnet.Except([pc2]).ToArray();
			var pc2NeighboursInSubnet = network[pc2].Intersect(subnetExceptPc2).ToArray();
		
			if (subnetExceptPc2.Any(x => !pc2NeighboursInSubnet.Contains(x)))
				return false;
		}

		return true;
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