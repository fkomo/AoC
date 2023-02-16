using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2020_18
{
	[AoCPuzzle(Year = 2020, Day = 18, Answer1 = "31142189909908", Answer2 = null)]
	public class OperationOrder : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			string answer1 = null, answer2 = null;

			// part1
			answer1 = input.Sum(line => Eval(line.Replace(" ", null))).ToString();

			// part2

			return (answer1, answer2);
		}

		public static long Eval(string expression)
		{
			Debug.Line(expression);

			var i = 1;

			long left;
			if (expression[0] == '(')
			{
				var closingBracket = expression.IndexOfClosingBracket(0);
				left = Eval(expression[1..closingBracket]);
				i += closingBracket;
			}
			else
				left = expression[0] - '0';

			for (; i < expression.Length; i += 2)
			{
				var op = expression[i];

				long right;
				if (char.IsDigit(expression[i + 1]))
					right = expression[i + 1] - '0';

				else// if (expression[i + 1] == '(')
				{
					var iEnd = expression.IndexOfClosingBracket(i + 1);
					right = Eval(expression[(i + 2)..iEnd]);
					i = iEnd - 1;
				}

				switch (op)
				{
					case '*': left *= right; break;
					case '+': left += right; break;
				}
			}

			return left;
		}
	}
}
