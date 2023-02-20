using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_08
{
	[AoCPuzzle(Year = 2015, Day = 08, Answer1 = "1371", Answer2 = null)]
	public class Matchsticks : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			string answer2 = null;

			// part1
			var answer1 = input.Sum(i => i.Length);
			foreach (var str in input)
			{
				var str2 = str[1..^1];

				var c1 = CountOccurances(str2, "\\\"");
				var c2 = CountOccurances(str2, "\\\\");
				var c3 = CountEscapedASCIIChars(str2);
				var numOfCharactersInMem = str2.Length - c1 - c2 - c3 * 3;

				answer1 -= numOfCharactersInMem;
			}

			// part2

			return (answer1.ToString(), answer2);
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
