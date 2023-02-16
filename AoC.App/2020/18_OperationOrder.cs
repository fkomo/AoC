using Ujeby.Alg;
using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2020_18
{
	[AoCPuzzle(Year = 2020, Day = 18, Answer1 = "31142189909908", Answer2 = "323912478287549")]
	public class OperationOrder : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			string answer1 = null, answer2 = null;

			var compressedInput = input.Select(line => line.Replace(" ", null)).ToArray();

			// part1
			answer1 = compressedInput.Sum(line => EvalLeftToRight(line)).ToString();

			// part2
			// nest all additions in brackets and eval left to right
			answer2 = compressedInput.Sum(line => EvalLeftToRight(AdditionsInBrackets(line))).ToString();

			return (answer1, answer2);
		}

		private static string AdditionsInBrackets(string expression)
		{
			for (var i = 0; i < expression.Length; i++)
			{
				if (expression[i] == '+')
				{
					// add left bracket
					var left = char.IsDigit(expression[i - 1]) ? i - 1 : expression.IndexOfOpeningBracket(i - 1);
					expression = expression[0..left] + "(" + expression[left..];
					
					i++;

					// add right bracket
					var right = char.IsDigit(expression[i + 1]) ? i + 2 : expression.IndexOfClosingBracket(i + 1) + 1;
					expression = expression[0..right] + ")" + expression[right..];
				}
			}

			return expression;
		}

		public static long EvalLeftToRight(string expression)
		{
			Debug.Line(expression);

			var i = 1;

			long left;
			if (expression[0] == '(')
			{
				var closingBracket = expression.IndexOfClosingBracket(0);
				left = EvalLeftToRight(expression[1..closingBracket]);
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
					right = EvalLeftToRight(expression[(i + 2)..iEnd]);
					i = iEnd - 1;
				}

				left = Eval(left, right, op);
			}

			Debug.Line($"={left}");
			return left;
		}

		private static long Eval(long left, long right, char op) => op switch
		{
			'*' => left * right,
			'+' => left + right,
			_ => throw new NotImplementedException(op.ToString())
		};
	}
}
