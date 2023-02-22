using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_08
{
	[AoCPuzzle(Year = 2015, Day = 08, Answer1 = "1371", Answer2 = "2117")]
	public class Matchsticks : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = input.Sum(i => i.Length) - input.Select(i => i[1..^1])
				.Sum(i => i.Length - CountOccurances(i, "\\\"") - CountOccurances(i, "\\\\") - CountEscapedASCIIChars(i) * 3);

			// part2
			var answer2 = input.Sum(i => i.Replace("\\", "\\\\").Replace("\"", "\\\"").Length + 2) - input.Sum(i => i.Length);

			return (answer1.ToString(), answer2.ToString());
		}

		private static int CountOccurances(string s, string value)
		{
			var next = 0;
			var count = 0;
			do
			{
				next = s.IndexOf(value, next);
				if (next != -1)
				{
					count++;
					next += value.Length;
				}
			}
			while (next != -1);
			return count;
		}

		private static int CountEscapedASCIIChars(string s)
		{
			var next = 0;
			var count = 0;
			do
			{
				next = s.IndexOf("\\x", next);
				if (next != -1)
				{
					next += 2;
					if ((next + 2) <= s.Length)
					{
						var value = s.Substring(next, 2);
						if (byte.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out _))
						{
							next += 2;
							count++;
						}
					}
				}
			}
			while (next != -1);
			return count;
		}
	}
}
