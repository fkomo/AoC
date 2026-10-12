using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2016_07;

[AoCPuzzle(Year = 2016, Day = 07, Answer1 = "105", Answer2 = "258", Skip = false)]
public class InternetProtocolVersion7 : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var ipv7 = input.Select(x => SplitIpv7(x)).ToArray();

		// part1
		var answer1 = ipv7.Count(x => SupportsTLS(x));

		// part2
		var answer2 = ipv7.Count(x => SupportsSSL(x));

		return (answer1.ToString(), answer2.ToString());
	}

	public static string[] SplitIpv7(string ipv7)
	{
		var start = 0;
		var parts = new List<string>();
		for (var i = 0; i < ipv7.Length; i++)
		{
			if (ipv7[i] == '[')
			{
				parts.Add(ipv7[start..i]);
				start = i;
			}
			else if (ipv7[i] == ']')
			{
				parts.Add(ipv7[start..(i + 1)]);
				start = i + 1;
			}
		}

		if (start < ipv7.Length)
			parts.Add(ipv7[start..ipv7.Length]);

		return parts.ToArray();
	}

	public static bool SupportsTLS(string[] ipv7)
		=> !ipv7.Any(x => x[0] == '[' && HasAABB(x)) && ipv7.Any(x => x[0] != '[' && HasAABB(x));

	private static bool HasAABB(string ipv7Part)
	{
		for (var i = 0; i < ipv7Part.Length - 3; i++)
		{
			var a = ipv7Part[i + 0];
			var b = ipv7Part[i + 1];
			if (a == b || !char.IsLetter(ipv7Part[i + 2]) || !char.IsLetter(ipv7Part[i + 3]))
				continue;

			if (b != ipv7Part[i + 2] || a != ipv7Part[i + 3])
				continue;

			return true;
		}

		return false;
	}

	public static bool SupportsSSL(string[] ipv7)
	{
		foreach (var net in ipv7.Where(x => x[0] != '['))
		{
			for (var i = 0; i < net.Length - 2; i++)
			{
				if (net[i + 0] == net[i + 1] || net[i + 0] != net[i + 2])
					continue;

				if (ipv7.Any(x => x[0] == '[' && x.Contains(new string(new char[] { net[i + 1], net[i], net[i + 1] }))))
					return true;
			}
		}

		return false;
	}
}