using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_01
{
	[AoCPuzzle(Year = 2017, Day = 01, Answer1 = "1182", Answer2 = "1152", Skip = false)]
	public class InverseCaptcha : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var seq = input.Single();

			//SumNext("1122");
			//SumNext("1111");
			//SumNext("1234");
			//SumNext("91212129");

			//SumHalfwayAround("1212");
			//SumHalfwayAround("1221");
			//SumHalfwayAround("123425");
			//SumHalfwayAround("123123");
			//SumHalfwayAround("12131415");

			// part1
			var answer1 = SumNext(seq);

			// part2
			var answer2 = SumHalfwayAround(seq);

			return (answer1.ToString(), answer2.ToString());
		}

		private static long SumNext(string sequence)
		{
			sequence += sequence[0];

			var sum = 0;
			for (var i = 0; i < sequence.Length - 1; i++)
				if (sequence[i] == sequence[i + 1])
					sum += sequence[i] - '0';

			Debug.Line($"{nameof(SumNext)}({sequence}): {sum}");
			return sum;
		}

		private static long SumHalfwayAround(string sequence)
		{
			var sum = 0;
			for (var i = 0; i < sequence.Length; i++)
				if (sequence[i] == sequence[(i + sequence.Length / 2) % sequence.Length])
					sum += sequence[i] - '0';

			Debug.Line($"{nameof(SumHalfwayAround)}({sequence}): {sum}");
			return sum;
		}
	}
}
