using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2020_09
{
	[AoCPuzzle(Year = 2020, Day = 09, Answer1 = "1038347917", Answer2 = "137394018")]
	public class EncodingError : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var n = input.Select(i => long.Parse(i)).ToArray();

			var preamble = 25;
			// sample
			//var preamble = 5;

			// part1
			long? answer1 = FirstInvalid(n, preamble);

			// part2
			long? answer2 = FindContaiguousSet(n, answer1.Value);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long FirstInvalid(long[] n, 
			int preamble = 25)
		{
			for (var i = preamble; i < n.Length; i++)
			{
				var valid = false;
				for (var p1 = i - 1; !valid && p1 >= i - preamble; p1--)
				{
					for (var p2 = p1 - 1; !valid && p2 >= i - preamble; p2--)
					{
						if (n[p1] + n[p2] == n[i])
							valid = true;
					}
				}

				if (!valid)
					return n[i];
			}

			return -1;
		}

		private static long FindContaiguousSet(long[] n, long invalid)
		{
			for (var start = 0; start < n.Length; start++)
			{
				var sum = n[start];
				for (var end = start + 1; end < n.Length; end++)
				{
					sum += n[end];
					if (sum == invalid)
					{
						var set = n.Skip(start).Take(end - start + 1);
						return set.Min() + set.Max();
					}
					else if (sum > invalid)
						break;
				}
			}

			return -1;
		}
	}
}
