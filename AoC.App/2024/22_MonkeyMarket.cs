using Ujeby.AoC.Common;
using Ujeby.Tools;

namespace Ujeby.AoC.App._2024_22;

[AoCPuzzle(Year = 2024, Day = 22, Answer1 = "20068964552", Answer2 = "2246", Skip = true)]
public class MonkeyMarket : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var inits = input.Select(long.Parse).ToArray();

		// part1
		var answer1 = inits.Sum(x => x.GetNthSecret());

		// part2
		// TODO OPTIMIZE 2024/22 p2 (2s)
		var trends = new Dictionary<int[], List<int>>(new IntArrayComparer());
		foreach (var init in inits)
			trends.ProcessPriceChanges(init);

		var answer2 = trends.Select(x => x.Value.Sum()).OrderByDescending(x => x).First();

		return (answer1.ToString(), answer2.ToString());
	}
}

static class Extensions
{
	public static long GetNthSecret(this long init, int n = 2000)
	{
		var secret = init;
		while (n-- > 0)
			secret = NextSecret(secret);

		return secret;
	}

	const int _changeSize = 4;

	public static Dictionary<int[], List<int>> ProcessPriceChanges(this Dictionary<int[], List<int>> trends, long secret, int n = 2000)
	{
		static int PriceFromSecret(long secret) => (int)(secret % 10);

		var prevPrice = PriceFromSecret(secret);

		var changes = new List<int>();

		var hs = new HashSet<int[]>(new IntArrayComparer());
		for (var i = 1; i <= n; i++)
		{
			secret = NextSecret(secret);

			var price = PriceFromSecret(secret);
			changes.Add(price - prevPrice);

			if (i >= _changeSize)
			{
				var trend = changes.TakeLast(_changeSize).ToArray();
				if (hs.Add(trend) && !trends.TryAdd(trend, [price]))
					trends[trend].Add(price);
			}

			prevPrice = price;
		}

		return trends;
	}

	static long NextSecret(long current)
	{
		static long MixAndPrune(long mix, long secret) => (mix ^ secret) % 16777216;

		current = MixAndPrune(current * 64, current);
		current = MixAndPrune(current / 32, current);
		current = MixAndPrune(current * 2048, current);

		return current;
	}
}