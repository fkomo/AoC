using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_21
{
	public class MonkeyMath : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
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
			long? answer1 = MonkeyYell("root", monkeys);

			// part2
			var humanSide = ContainsName((string)monkeys["root"][0], "humn", monkeys) ? 0 : 2;
			long? answer2 = MonkeyYell((string)monkeys["root"][(humanSide + 2) % 4], monkeys, human: 0);

			var op = (string)monkeys["root"][humanSide];
			var pathToHuman = PathTo((string)monkeys["root"][humanSide], "humn", monkeys).Reverse().ToArray();
			foreach (var path in pathToHuman)
			{
				if (path == 0)
				{
					var other = (string)monkeys[op][2];
					switch ((string)monkeys[op][1])
					{
						case "+": answer2 -= MonkeyYell(other, monkeys); break;
						case "-": answer2 += MonkeyYell(other, monkeys); break;
						case "*": answer2 /= MonkeyYell(other, monkeys); break;
						case "/": answer2 *= MonkeyYell(other, monkeys); break;
						default:
							break;
					}
				}
				else
				{
					var other = (string)monkeys[op][0];
					switch ((string)monkeys[op][1])
					{
						case "+": answer2 -= MonkeyYell(other, monkeys); break;
						case "-": answer2 = MonkeyYell(other, monkeys) - answer2; break;
						case "*": answer2 /= MonkeyYell(other, monkeys); break;
						case "/": answer2 = MonkeyYell(other, monkeys) / answer2; break;
						default:
							break;
					}
				}

				op = (string)monkeys[op][path];
			}

			// verification
			//if (!CheckHumanYell(answer2.Value, monkeys))
			//	answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private int[] PathTo(string monkey, string name, Dictionary<string, object[]> monkeys)
		{
			if (monkeys[monkey].Length == 3)
			{
				if ((string)monkeys[monkey][0] == name)
					return new int[] { 0 };

				else if ((string)monkeys[monkey][2] == name)
					return new int[] { 2 };              
					
				var left = PathTo((string)monkeys[monkey][0], name, monkeys);
				if (left.Any())
					return left.Concat(new int[] { 0 }).ToArray();

				var right = PathTo((string)monkeys[monkey][2], name, monkeys);
				if (right.Any())
					return right.Concat(new int[] { 2 }).ToArray();
			}

			return Array.Empty<int>();
		}

		private static long MonkeyYell(string name, Dictionary<string, object[]> monkeys,
			long? human = null)
		{
			if (name == "humn" && human.HasValue)
				return human.Value;

			var yell = monkeys[name];
			if (yell.Length == 1)
				return (long)yell[0];

			var left = MonkeyYell((string)yell[0], monkeys, human: human);
			var right = MonkeyYell((string)yell[2], monkeys, human: human);

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

		private static bool ContainsName(string monkey, string name, Dictionary<string, object[]> monkeys)
		{
			if (monkey == name)
				return true;

			if (monkeys[monkey].Length == 1)
				return false;

			return ContainsName((string)monkeys[monkey][0], name, monkeys) || ContainsName((string)monkeys[monkey][2], name, monkeys);
		}

		private static bool CheckHumanYell(long human, Dictionary<string, object[]> monkeys)
		{
			var left = MonkeyYell((string)monkeys["root"][0], monkeys, human: human);
			var right = MonkeyYell((string)monkeys["root"][2], monkeys, human: human);

			return left == right;
		}
	}
}
