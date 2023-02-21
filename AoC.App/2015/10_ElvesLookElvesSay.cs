using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_10
{
	[AoCPuzzle(Year = 2015, Day = 10, Answer1 = "492982", Answer2 = "6989950")]
	public class ElvesLookElvesSay : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = LookAndSay(input.Single(), 40);

			// part2
			var answer2 = LookAndSay(answer1, 10);

			return (answer1.Length.ToString(), answer2.Length.ToString());
		}

		private static string LookAndSay(string start, int stepCount)
		{
			var sb = new StringBuilder();
			while (stepCount-- > 0)
			{
				for (var i = 0; i <= start.Length - 1; i++)
				{
					var cnt = 1;
					var n = start[i];
					while (i + cnt < start.Length && n == start[i + cnt])
						cnt++;

					sb.Append($"{cnt}{n}");
					i += cnt - 1;
				}

				start = sb.ToString();
				sb.Clear();
			}

			return start;
		}
	}
}
