using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_11
{
	[AoCPuzzle(Year = 2022, Day = 11, Answer1 = "62491", Answer2 = "17408399184")]
	public class MonkeyInTheMiddle : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long? answer1 = MonkeyBusiness(CreateMonkeys(input).ToArray(), 20, "old / 3");

			// part2
			var monkeys = CreateMonkeys(input).ToArray();
			long manage = 1;
			foreach (var (items, op, test, next) in monkeys)
				manage *= test;
			long? answer2 = MonkeyBusiness(monkeys, 10000, $"old % {manage}");

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long MonkeyBusiness((List<long> items, string op, long test, int[] next)[] monkeys, int rounds, string relief)
		{
			var inspects = new long[monkeys.Length];

			for (var r = 0; r < rounds; r++)
			{
				for (var m = 0; m < monkeys.Length; m++)
				{
					for (var i = 0; i < monkeys[m].items.Count; i++)
					{
						var newItemLevel = GetItemLevel(relief, GetItemLevel(monkeys[m].op, monkeys[m].items[i]));
						if (newItemLevel % monkeys[m].test == 0)
							monkeys[monkeys[m].next[0]].items.Add(newItemLevel);

						else
							monkeys[monkeys[m].next[1]].items.Add(newItemLevel);
					}

					inspects[m] += monkeys[m].items.Count;
					monkeys[m].items.Clear();
				}
			}

			var monkeyBusiness = inspects.OrderByDescending(n => n).ToArray();
			return monkeyBusiness[0] * monkeyBusiness[1];
		}

		private static List<(List<long> items, string op, long test, int[] next)> CreateMonkeys(string[] input)
		{
			var monkeys = new List<(List<long> items, string op, long test, int[] next)>();
			for (var l = 0; l < input.Length; l += 7)
			{
				monkeys.Add(
					(
						input[l + 1]["  Starting items: ".Length..].Split(", ").Select(i => long.Parse(i)).ToList(),
						input[l + 2]["  Operation: new = ".Length..],
						int.Parse(input[l + 3]["  Test: divisible by ".Length..]),
						new int[]
						{
							int.Parse(input[l + 4]["    If true: throw to monkey ".Length..]),
							int.Parse(input[l + 5]["    If false: throw to monkey ".Length..])
						}
					));
			}

			return monkeys;
		}

		private static long GetItemLevel(string op, long itemLevel)
		{
			var value = op["old * ".Length..];
			if (op.Contains(" * ")) 
			{
				if (value == "old")
					return itemLevel * itemLevel;
				else
					return itemLevel * int.Parse(value);
			}
			else if (op.Contains(" + "))
				return itemLevel + int.Parse(value);

			else if (op.Contains(" / "))
				return itemLevel / long.Parse(value);

			else if (op.Contains(" % "))
				return itemLevel % long.Parse(value);

			throw new NotImplementedException();
		}
	}
}
