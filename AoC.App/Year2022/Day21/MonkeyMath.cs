using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day21
{
	public class MonkeyMath : PuzzleBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var monkeys = input.ToDictionary(
				line => line[..line.IndexOf(':')],
				line =>
				{
					var split = line[(line.IndexOf(": ") + ": ".Length)..].Split(' ');
					if (split.Length == 1)
						return new object[] { long.Parse(split[0]) };

					return new object[] { split[0], split[1], split[2] };
				});

			// part1
			long? answer1 = MonkeyYellSimple("root", monkeys);

			// part2
			long? answer2 = null;
			long human = 301; // sample
			// TODO 2022/21 part2
			if (CheckHuman(human, monkeys))
				answer2 = human;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long MonkeyYellSimple(string name, Dictionary<string, object[]> monkeys,
			long? human = null)
		{
			if (name == "humn" && human.HasValue)
				return human.Value;

			var yell = monkeys[name];
			if (yell.Length == 1)
				return (long)yell[0];

			var left = MonkeyYellSimple((string)yell[0], monkeys, human: human);
			var right = MonkeyYellSimple((string)yell[2], monkeys, human: human);

			switch ((string)yell[1])
			{
				case "+": return left + right;
				case "-": return left - right;
				case "*": return left * right;
				case "/": return left / right;
				default:
					break;
			}

			throw new NotImplementedException();
		}

		private static bool CheckHuman(long human, Dictionary<string, object[]> monkeys)
		{
			var left = MonkeyYellSimple((string)monkeys["root"][0], monkeys, human: human);
			var right = MonkeyYellSimple((string)monkeys["root"][2], monkeys, human: human);

			return left == right;
		}
	}
}
