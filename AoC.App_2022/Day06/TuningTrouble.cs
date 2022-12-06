using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day06
{
	internal class TuningTrouble : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long? answer1 = FindDistinctSequence(input[0], 4);

			// part2
			long? answer2 = FindDistinctSequence(input[0], 14);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long FindDistinctSequence(string data, int length)
		{
			long result = 1;
			for (long sequenceStart = 0; result - sequenceStart < length; result++)
				for (long c = result - 1; c >= sequenceStart; c--)
					if (data[(int)c] == data[(int)result])
					{
						sequenceStart = c + 1;
						break;
					}

			return result;
		}
	}
}
