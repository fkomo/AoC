using Ujeby.AoC.Common;
using Ujeby.Tools;

namespace Ujeby.AoC.App._2021_18
{
	[AoCPuzzle(Year = 2021, Day = 18, Answer1 = "4641", Answer2 = "4624")]
	public class Snailfish : PuzzleBase
	{
		public class SnailfishNum
		{
			public int? Left;
			public int? Right;

			public SnailfishNum LeftSnail;
			public SnailfishNum RightSnail;

			public SnailfishNum(string input)
			{
				var s = input[1..^1];
				if (s.Count(c => c == ',') == 1)
				{
					var split = s.Split(',');
					Left = int.Parse(split[0]);
					Right = int.Parse(split[1]);
				}
				else
				{
					var split = s.SplitNestedArrays();

					if (!split[0].Contains(','))
						Left = int.Parse(split[0]);
					else
						LeftSnail = new(split[0]);

					if (!split[1].Contains(','))
						Right = int.Parse(split[1]);
					else
						RightSnail = new(split[1]);
				}
			}

			public SnailfishNum(int left, int right)
			{
				Left = left;
				Right = right;
			}

			public int Magnitude => 
				3 * (Left ?? LeftSnail.Magnitude) + 2 * (Right ?? RightSnail.Magnitude);

			public override string ToString() =>
				$"[{Left?.ToString() ?? LeftSnail.ToString()},{Right?.ToString() ?? RightSnail.ToString()}]";

			public bool Split()
			{
				if (Left >= 10)
				{
					LeftSnail = new(
						(int)Math.Floor((decimal)Left.Value / 2),
						(int)Math.Ceiling((decimal)Left / 2));

					Left = null;
					return true;
				}
				else if (LeftSnail?.Split() == true)
					return true;

				if (Right >= 10)
				{
					RightSnail = new(
						(int)Math.Floor((decimal)Right.Value / 2),
						(int)Math.Ceiling((decimal)Right / 2));

					Right = null;
					return true;
				}
				else if (RightSnail?.Split() == true)
					return true;

				return false;
			}
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			var n = input[0];
			for (var i = 1; i < input.Length; i++)
				n = Reduce($"[{n},{input[i]}]");

			// part1
			long? answer1 = new SnailfishNum(n).Magnitude;

			// part2
			long? answer2 = long.MinValue;
			for (var n1 = 0; n1 < input.Length; n1++)
			{
				for (var n2 = 0; n2 < input.Length; n2++)
				{
					if (n1 == n2)
						continue;

					var magnitude = new SnailfishNum(Reduce($"[{input[n1]},{input[n2]}]")).Magnitude;
					if (magnitude > answer2)
						answer2 = magnitude;
				}
			}

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static string Reduce(string num)
		{
			while (true)
			{
				if (Explode(num, out num))
					continue;

				if (Split(num, out num))
					continue;

				break;
			}

			return num;
		}

		private static bool Split(string num, out string splitted)
		{
			var snail = new SnailfishNum(num);

			var result = false;
			if (snail.Split())
				result = true;

			splitted = snail.ToString();
			return result;
		}

		private static bool Explode(string num, out string exploded)
		{
			var nest = 0;
			for (var i = 0; i < num.Length; i++)
			{
				if (num[i] == '[')
					nest++;

				else if (num[i] == ']')
					nest--;

				if (nest == 5)
				{
					var i2 = i + 1;
					for (; num[i2] != ']'; i2++)
					{
					}

					var pair = num.Substring(i, i2 - i + 1);
					var values = pair[1..^1].Split(',').Select(s => int.Parse(s)).ToArray();

					var rightFrom = i2 + 1;
					for (; rightFrom < num.Length && !char.IsDigit(num[rightFrom]); rightFrom++)
					{
					}
					if (rightFrom < num.Length - 1)
					{
						var rightTo = rightFrom + 1;
						for (; rightTo < num.Length - 1 && char.IsDigit(num[rightTo]); rightTo++)
						{
						}
						if (rightTo < num.Length)
						{
							var right = int.Parse(num[rightFrom..rightTo]);
							right += values[1];
							num = num[..rightFrom] + right + num.Substring(rightTo);
						}
					}

					num = string.Concat(num.AsSpan(0, i), "0", num.AsSpan(i2 + 1));

					var leftTo = i - 1;
					for (; leftTo >= 0 && !char.IsDigit(num[leftTo]); leftTo--)
					{
					}
					if (leftTo > 0)
					{
						var leftFrom = leftTo - 1;
						for (; leftFrom >= 0 && char.IsDigit(num[leftFrom]); leftFrom--)
						{
						}

						if (leftFrom > 0)
						{
							var left = int.Parse(num.Substring(leftFrom + 1, leftTo - leftFrom));
							left += values[0];
							num = num[..(leftFrom + 1)] + left + num[(leftTo + 1)..];
						}
					}

					exploded = num;
					return true;
				}
			}

			exploded = num;
			return false;
		}
	}
}
