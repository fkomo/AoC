using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day21
{
	public class MonkeyMath : PuzzleBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var monkeys = input.ToDictionary(
				line => line[..line.IndexOf(':')], 
				line => line[(line.IndexOf(": ") + ": ".Length)..]);

			// part1
			long? answer1 = MonkeyYell("root", monkeys);

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long MonkeyYell(string name, Dictionary<string, string> monkeys)
		{
			var yell = monkeys[name];
			if (long.TryParse(yell, out long result))
				return result;

			var split = yell.Split(' ');
			var left = MonkeyYell(split[0], monkeys);
			var right = MonkeyYell(split[2], monkeys);

			switch (split[1])
			{
				case "+": return left + right;
				case "-": return left - right;
				case "*": return left * right;
				case "/": return left / right;
			}

			throw new NotImplementedException();
		}
	}
}
