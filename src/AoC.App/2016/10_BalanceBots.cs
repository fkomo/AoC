using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2016_10;

[AoCPuzzle(Year = 2016, Day = 10, Answer1 = "56", Answer2 = "7847", Skip = false)]
public class BalanceBots : PuzzleBase
{
	public const int Bot = -1;
	public const int Output = -2;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var bots = input.Where(i => i.StartsWith("value"))
			.Select(x => x.ToNumArray())
			.GroupBy(x => x.Last())
			.ToDictionary(x => x.Key, x => (Chips: x.Select(v => v.First()).ToList(), Cmps: new List<(long Min, long Max)>()));

		var instr = input.Where(x => x[0] == 'b')
			.Select(x => x.Replace(" output ", $" {Output} ").Replace(" bot ", $" {Bot} ").ToNumArray())
			.ToArray();

		// part1
		var output = new Dictionary<long, List<long>>();
		while (bots.Any(b => b.Value.Chips.Count == 2))
		{
			var bots2 = bots.Where(x => x.Value.Chips.Count == 2).Select(x => x.Key).ToArray();
			foreach (var b in bots2)
			{
				var param = instr.Single(x => x[0] == b);

				var min = bots[b].Chips.Min();
				var max = bots[b].Chips.Max();
				bots[b].Cmps.Add((Min: min, Max: max));

				// low
				switch (param[1])
				{
					case Bot: GiveChipTo(b, param[2], min, bots, output); break;
					case Output: GiveChipTo(b, param[2], min, bots, output, toBot: false); break;
				}

				// high
				switch (param[3])
				{
					case Bot: GiveChipTo(b, param[4], max, bots, output); break;
					case Output: GiveChipTo(b, param[4], max, bots, output, toBot: false); break;
				}
			}
		}

		// sample
		//var answer1 = bots.Single(x => x.Value.Cmps.Contains((Min: 2, Max: 5))).Key;
		var answer1 = bots.Single(x => x.Value.Cmps.Contains((Min: 17, Max: 61))).Key;

		// part2
		var answer2 = output.Single(x => x.Key == 0).Value.Single() *
			output.Single(x => x.Key == 1).Value.Single() *
			output.Single(x => x.Key == 2).Value.Single();

		return (answer1.ToString(), answer2.ToString());
	}

	private static void GiveChipTo(long from, long to, long chip, 
		Dictionary<long, (List<long> Chips, List<(long Min, long Max)> Cmps)> bots, Dictionary<long, List<long>> output,
		bool toBot = true)
	{
		bots[from].Chips.Remove(chip);

		if (toBot)
		{
			if (!bots.ContainsKey(to))
				bots.Add(to, (Chips: new List<long>(), Cmps: new List<(long Min, long Max)>()));

			bots[to].Chips.Add(chip);
		}
		else
		{
			if (!output.ContainsKey(to))
				output.Add(to, new List<long>());

			output[to].Add(chip);
		}
	}
}